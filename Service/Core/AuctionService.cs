using AutoMapper;
using Data.EFCore;
using Data.Entities;
using Data.Enum;
using Data.Models;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RealEstateAuctionManagement.Extensions;
using Service.SignalR;
using Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core
{
    public interface IAuctionService
    {
        Task<PagingModel<AuctionViewModel>> GetAll(AuctionQueryModel query, string userRole);
        Task<PagingModel<AuctionViewModel>> GetOwnAuctions(AuctionQueryModel query, string userId);
        Task<AuctionViewModel> GetById(Guid id);
        Task<Guid> Create(AuctionCreateModel auctionCreateModel);
        Task<Guid> Update(Guid id, AuctionUpdateModel model, string userId);
        Task<Guid> Delete(Guid id, string userId);
        Task<Guid> CreateAuctionRequest(AuctionCreateRequestModel auctionCreateModel, string userId);
        Task<Guid> ApproveAuction(Guid id, ApproveAuctionModel model, string approvedById);
        Task<string> RegisterForAuction(Guid id, string userId);
        Task<PaymentResponseModel> PaymentCallback(IQueryCollection collection);
        Task<Guid> PlaceBid(Guid auctionId, PlaceBidModel model, string userId);
        Task<Guid> OpenAuction(Guid id);
        Task<Guid> CloseAuction(Guid id);
    }

    public class AuctionService : IAuctionService
    {
        private readonly DataContext _dataContext;
        private ISortHelpers<Auction> _sortHelper;
        private readonly IMapper _mapper;
        private readonly IVnPayService _vnPayService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHubContext<SignalRHub> _hubContext;
        private static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        public AuctionService(DataContext dataContext, ISortHelpers<Auction> sortHelper, IMapper mapper, IVnPayService vnPayService, IHttpContextAccessor httpContextAccessor, IHubContext<SignalRHub> hubContext)
        {
            _dataContext = dataContext;
            _sortHelper = sortHelper;
            _mapper = mapper;
            _vnPayService = vnPayService;
            _httpContextAccessor = httpContextAccessor;
            _hubContext = hubContext;
        }

        public async Task<Guid> Create(AuctionCreateModel model)
        {
            try
            {
                var existedAuction = await _dataContext.Auctions
                    .Where(x => !x.IsDeleted)
                    .FirstOrDefaultAsync();
                var data = _mapper.Map<AuctionCreateModel, Auction>(model);
                await _dataContext.Auctions.AddAsync(data);
                await _dataContext.SaveChangesAsync();
                return data.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<PagingModel<AuctionViewModel>> GetAll(AuctionQueryModel query, string userRole)
        {
            try
            {
                var queryData = _dataContext.Auctions
                    .Where(x => !x.IsDeleted);

                if (string.IsNullOrEmpty(userRole) || userRole == Role.Member.ToString())
                {
                    queryData = queryData
                        .Where(x => x.Status != AuctionStatus.Pending && x.Status != AuctionStatus.Rejected);
                }

                var sortData = _sortHelper.ApplySort(queryData, query.OrderBy!);

                var data = await sortData.ToPagedListAsync(query.PageIndex, query.PageSize);

                var pagingData = new PagingModel<AuctionViewModel>()
                {
                    PageIndex = data.CurrentPage,
                    PageSize = data.PageSize,
                    TotalCount = data.TotalCount,
                    pagingData = _mapper.Map<List<Auction>, List<AuctionViewModel>>(data)
                };
                return pagingData;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<AuctionViewModel> GetById(Guid id)
        {
            try
            {
                var data = await GetAuction(id);
                if (data == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }

                return _mapper.Map<Auction, AuctionViewModel>(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<Guid> Update(Guid id, AuctionUpdateModel model, string userId)
        {
            try
            {
                var checkExistAuction = await GetAuction(id);
                if (checkExistAuction == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }

                var user = await _dataContext.Users
                    .FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == new Guid(userId));
                if (user == null)
                {
                    throw new AppException(ErrorMessage.UserNameDoNotExist);
                }

                // Check if the user is a member and if they own the auction
                if (user.Role == Role.Member && checkExistAuction.CreateByUserId != new Guid(userId))
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }

                // Check if the auction is in a pending status
                if (checkExistAuction.Status != AuctionStatus.Pending)
                {
                    throw new AppException(ErrorMessage.AuctionNotPending);
                }

                var realEstate = await _dataContext.RealEstates
                    .Where(x => !x.IsDeleted && x.Id == model.RealEstateId)
                    .FirstOrDefaultAsync();
                if (realEstate == null)
                {
                    throw new AppException(ErrorMessage.RealEstateNotExist);
                }

                // Check if user is member and they own the real estate
                if (user.Role == Role.Member && realEstate.UserId != user.Id)
                {
                    throw new AppException(ErrorMessage.RealEstateNotExist);
                }

                // Check if the real estate is approved
                if (realEstate.Status != RealEstateStatus.Approved)
                {
                    throw new AppException(ErrorMessage.RealEstateNotApproved);
                }

                // Check if the real estate is already in an auction
                var existingAuction = await _dataContext.Auctions
                    .Where(x => !x.IsDeleted && x.RealEstateId == model.RealEstateId && x.Id != id && x.Status != AuctionStatus.Rejected && x.Status != AuctionStatus.Failed)
                    .FirstOrDefaultAsync();
                if (existingAuction != null)
                {
                    throw new AppException(ErrorMessage.RealEstateAlreadyInAuction);
                }

                // Check if the start date is at least 7 days from now
                if (model.StartDate < DateTime.UtcNow.AddDays(7))
                {
                    throw new AppException(ErrorMessage.StartDateTooEarly);
                }

                // Check if the end date is the same as the start date
                if (model.EndDate!.Value.Date != model.StartDate!.Value.Date)
                {
                    throw new AppException(ErrorMessage.EndDateNotSameAsStartDate);
                }

                // Check if the end time is later than the start time
                if (model.EndDate!.Value.TimeOfDay <= model.StartDate!.Value.TimeOfDay)
                {
                    throw new AppException(ErrorMessage.EndTimeNotLaterThanStartTime);
                }

                // Check max bid increment
                if (model.MaxBidIncrement != null)
                {
                    if (model.MaxBidIncrement < model.BidIncrement)
                    {
                        throw new AppException(ErrorMessage.MaxBidIncrementLessThanBidIncrement);
                    }
                    if (model.MaxBidIncrement % model.BidIncrement != 0)
                    {
                        throw new AppException(ErrorMessage.MaxBidIncrementNotMultipleOfBidIncrement);
                    }
                }

                var updateData = _mapper.Map(model, checkExistAuction);
                _dataContext.Auctions.Update(updateData);
                await _dataContext.SaveChangesAsync();
                return checkExistAuction.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<Guid> Delete(Guid id, string userId)
        {
            try
            {
                var checkExistAuction = await GetAuction(id);
                if (checkExistAuction == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }

                var user = await _dataContext.Users
                    .FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == new Guid(userId));
                if (user == null)
                {
                    throw new AppException(ErrorMessage.UserNameDoNotExist);
                }

                // Check if the user is a member and if they own the auction
                if (user.Role == Role.Member && checkExistAuction.CreateByUserId != new Guid(userId))
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }

                // Check if the auction is in a pending status
                if (checkExistAuction.Status != AuctionStatus.Pending)
                {
                    throw new AppException(ErrorMessage.AuctionNotPending);
                }

                checkExistAuction.IsDeleted = true;
                _dataContext.Auctions.Update(checkExistAuction);
                await _dataContext.SaveChangesAsync();
                return checkExistAuction.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<Guid> CreateAuctionRequest(AuctionCreateRequestModel auctionCreateModel, string userId)
        {
            try
            {
                var realEstate = await _dataContext.RealEstates
                    .Where(x => !x.IsDeleted && x.Id == auctionCreateModel.RealEstateId && x.UserId == new Guid(userId))
                    .FirstOrDefaultAsync();
                if (realEstate == null)
                {
                    throw new AppException(ErrorMessage.RealEstateNotExist);
                }

                // Check if the real estate is approved
                if (realEstate.Status != RealEstateStatus.Approved)
                {
                    throw new AppException(ErrorMessage.RealEstateNotApproved);
                }

                // Check if the real estate is already in an auction
                var existingAuction = await _dataContext.Auctions
                    .Where(x => !x.IsDeleted && x.RealEstateId == auctionCreateModel.RealEstateId && x.Status != AuctionStatus.Rejected && x.Status != AuctionStatus.Failed)
                    .FirstOrDefaultAsync();
                if (existingAuction != null)
                {
                    throw new AppException(ErrorMessage.RealEstateAlreadyInAuction);
                }

                // Check if the start date is at least 7 days from now
                if (auctionCreateModel.StartDate < DateTime.UtcNow.AddDays(7))
                {
                    throw new AppException(ErrorMessage.StartDateTooEarly);
                }

                // Check if the end date is the same as the start date
                if (auctionCreateModel.EndDate!.Value.Date != auctionCreateModel.StartDate!.Value.Date)
                {
                    throw new AppException(ErrorMessage.EndDateNotSameAsStartDate);
                }

                // Check if the end time is later than the start time
                if (auctionCreateModel.EndDate!.Value.TimeOfDay <= auctionCreateModel.StartDate!.Value.TimeOfDay)
                {
                    throw new AppException(ErrorMessage.EndTimeNotLaterThanStartTime);
                }

                // Check max bid increment
                if (auctionCreateModel.MaxBidIncrement != null)
                {
                    if (auctionCreateModel.MaxBidIncrement < auctionCreateModel.BidIncrement)
                    {
                        throw new AppException(ErrorMessage.MaxBidIncrementLessThanBidIncrement);
                    }
                    if (auctionCreateModel.MaxBidIncrement % auctionCreateModel.BidIncrement != 0)
                    {
                        throw new AppException(ErrorMessage.MaxBidIncrementNotMultipleOfBidIncrement);
                    }
                }

                var auction = _mapper.Map<AuctionCreateRequestModel, Auction>(auctionCreateModel);

                // Get percent from settings
                var registrationFeePercent = float.Parse(_dataContext.Settings.FirstOrDefault(x => x.Key == "REGISTRATION_FEE_PERCENT")?.Value ?? "0") / 100;
                var depositPercent = float.Parse(_dataContext.Settings.FirstOrDefault(x => x.Key == "DEPOSIT_PERCENT")?.Value ?? "0") / 100;

                auction.CreateByUserId = new Guid(userId);
                auction.RegistrationFee = (float)Math.Round(auction.StartingPrice * registrationFeePercent);
                auction.Deposit = (float)Math.Round(auction.StartingPrice * depositPercent);

                await _dataContext.Auctions.AddAsync(auction);
                await _dataContext.SaveChangesAsync();

                return auction.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<Guid> ApproveAuction(Guid id, ApproveAuctionModel model, string approvedById)
        {
            try
            {
                var auction = await _dataContext.Auctions
                    .Include(x => x.RealEstates)
                    .FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);
                if (auction == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }

                // Check if the auction is pending
                if (auction.Status != AuctionStatus.Pending)
                {
                    throw new AppException(ErrorMessage.AuctionNotPending);
                }

                if (model.IsApproved)
                {
                    // Check if the current date is past two days before the start date of the auction
                    if (DateTime.UtcNow > auction.StartDate.AddDays(-2))
                    {
                        auction.Status = AuctionStatus.Rejected;
                        throw new AppException(ErrorMessage.ApprovalRequestExpired);
                    }

                    auction.Status = AuctionStatus.Approved;
                    auction.ApproveByUserId = new Guid(approvedById);
                    auction.ApproveTime = DateTime.UtcNow;
                    auction.RegistrationStartDate = DateTime.UtcNow;
                    auction.RegistrationEndDate = auction.StartDate.AddDays(-2);

                    // Schedule task for opening auction
                    var openJobId = BackgroundJob.Schedule<BackgroundServices>(s => s.OpenAuction(auction.Id), auction.StartDate);
                    KeyValueStore.Instance.Set($"OpenAuctionTask_{auction.Id}", openJobId);

                    // Schedule task for closing the auction at the end date time
                    var closeJobId = BackgroundJob.Schedule<BackgroundServices>(s => s.CloseAuction(auction.Id), auction.EndDate);
                    KeyValueStore.Instance.Set($"CloseAuctionTask_{auction.Id}", closeJobId);
                }
                else
                {
                    auction.Status = AuctionStatus.Rejected;
                }

                auction.DateUpdate = DateTime.UtcNow;

                _dataContext.Auctions.Update(auction);
                await _dataContext.SaveChangesAsync();

                var notification = new Notification
                {
                    Title = model.IsApproved ? "Phiên đấu giá đã được duyệt" : "Phiên đấu giá bị từ chối",
                    Description = model.IsApproved ? $"Yêu cầu đấu giá tài sản {auction.RealEstates.Name} của bạn đã được duyệt."
                        : $"Yêu cầu đấu giá tài sản {auction.RealEstates.Name} của bạn đã bị từ chối do thông tin cung cấp chưa phù hợp.",
                    UserId = auction.CreateByUserId
                };

                _dataContext.Notifications.Add(notification);
                await _dataContext.SaveChangesAsync();

                return auction.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<PagingModel<AuctionViewModel>> GetOwnAuctions(AuctionQueryModel query, string userId)
        {
            try
            {
                var queryData = _dataContext.Auctions
                    .Where(x => !x.IsDeleted && x.CreateByUserId == new Guid(userId));

                var sortData = _sortHelper.ApplySort(queryData, query.OrderBy!);

                var data = await sortData.ToPagedListAsync(query.PageIndex, query.PageSize);

                var pagingData = new PagingModel<AuctionViewModel>()
                {
                    PageIndex = data.CurrentPage,
                    PageSize = data.PageSize,
                    TotalCount = data.TotalCount,
                    pagingData = _mapper.Map<List<Auction>, List<AuctionViewModel>>(data)
                };
                return pagingData;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<string> RegisterForAuction(Guid id, string userId)
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext!;

                var auction = await _dataContext.Auctions
                    .FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);
                if (auction == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }

                // Check if the auction is approved
                if (auction.Status != AuctionStatus.Approved)
                {
                    throw new AppException(ErrorMessage.RealEstateNotApproved);
                }

                // Check if the user is the owner of the auction
                if (auction.CreateByUserId == new Guid(userId))
                {
                    throw new AppException(ErrorMessage.UserCannotRegisterOwnAuction);
                }

                // Check if the user has already registered for this auction
                var existingRegistration = await _dataContext.UserBids
                    .FirstOrDefaultAsync(x => x.UserId == new Guid(userId) && x.AuctionId == id && x.IsDeposit);
                if (existingRegistration != null)
                {
                    throw new AppException(ErrorMessage.UserAlreadyRegisteredAuction);
                }

                // Calculate the fee
                var amount = auction.RegistrationFee + auction.Deposit;

                var transaction = new Transaction
                {
                    Id = Guid.NewGuid(),
                    Amount = amount,
                    PaymentMethod = PaymentMethod.VnPay,
                    Status = TransactionStatus.Pending,
                    Type = TransactionType.DepositFee,
                    UserId = new Guid(userId),
                    AuctionId = id
                };

                await _dataContext.Transaction.AddAsync(transaction);
                await _dataContext.SaveChangesAsync();

                var response = await _vnPayService.CreatePaymentUrl(userId, amount, transaction.Id, httpContext);
                if (response.Equals(""))
                {
                    throw new AppException(ErrorMessage.PaymentRequestCreationError);
                }

                // Set schedule to fail the transaction in 15 minutes if still pending
                BackgroundJob.Schedule<BackgroundServices>(s => s.CheckTransactionStatus(transaction.Id), TimeSpan.FromMinutes(15));
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<PaymentResponseModel> PaymentCallback(IQueryCollection collection)
        {
            var callbackResponse = _vnPayService.PaymentExecute(collection);
            var transaction = await _dataContext.Transaction.FirstOrDefaultAsync(x => x.Id == new Guid(callbackResponse.OrderId));
            if (transaction == null)
            {
                throw new AppException(ErrorMessage.TransactionNotExist);
            }

            if (!transaction.Status.Equals(TransactionStatus.Pending))
            {
                throw new AppException(ErrorMessage.TransactionClosed);
            }
            else
            {
                //VNPAY RETURN SUCCESS STATUS
                if (callbackResponse.VnPayResponseCode.Equals("00"))
                {
                    transaction.Status = TransactionStatus.Successful;
                    transaction.DateUpdate = DateTime.UtcNow;

                    var userBid = new UserBid
                    {
                        Amount = transaction.Amount,
                        IsDeposit = true,
                        UserId = transaction.UserId,
                        AuctionId = (Guid)transaction.AuctionId!
                    };
                    await _dataContext.AddAsync(userBid);
                    await _dataContext.SaveChangesAsync();
                }
                //VNPAY RETURN FAILED STATUS
                else
                {
                    //MARK TRANSACTION AS FAILED
                    transaction.Status = TransactionStatus.Failed;
                    transaction.DateUpdate = DateTime.UtcNow;
                }
                _dataContext.Transaction.Update(transaction);
                _dataContext.SaveChanges();
                return callbackResponse;
            }
        }

        public async Task<Guid> PlaceBid(Guid auctionId, PlaceBidModel model, string userId)
        {
            try
            {
                // Wait to enter the semaphore.
                await semaphoreSlim.WaitAsync();

                var auction = await _dataContext.Auctions
                    .FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == auctionId);
                if (auction == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }

                // Check if the auction is ongoing
                if (auction.Status != AuctionStatus.OnGoing)
                {
                    throw new AppException(ErrorMessage.AuctionNotOngoing);
                }

                // Check if the user is the owner of the auction
                if (auction.CreateByUserId == new Guid(userId))
                {
                    throw new AppException(ErrorMessage.UserCannotBidOwnAuction);
                }

                // Retrieve the highest bid for this auction
                var highestBid = await _dataContext.UserBids
                    .Where(x => x.AuctionId == auctionId && !x.IsDeposit)
                    .OrderByDescending(x => x.Amount)
                    .FirstOrDefaultAsync();

                // Check if the new bid is higher than the current highest bid
                if (highestBid != null && model.Amount <= highestBid.Amount)
                {
                    throw new AppException(ErrorMessage.BidNotHigherThanCurrent);
                }

                // Create a new bid
                var userBid = new UserBid
                {
                    Amount = model.Amount,
                    IsDeposit = false,
                    UserId = new Guid(userId),
                    AuctionId = auctionId
                };

                await _dataContext.UserBids.AddAsync(userBid);
                await _dataContext.SaveChangesAsync();

                // Create a new transaction
                var transaction = new Transaction
                {
                    Id = Guid.NewGuid(),
                    Amount = model.Amount,
                    PaymentMethod = PaymentMethod.None,
                    Status = TransactionStatus.Successful,
                    Type = TransactionType.Bid,
                    UserId = new Guid(userId),
                    AuctionId = auctionId
                };

                await _dataContext.Transaction.AddAsync(transaction);
                await _dataContext.SaveChangesAsync();

                // Send SignalR message to update highest bid
                await _hubContext.Clients.Group(auctionId.ToString()).SendAsync("UpdateHighestBid", model.Amount);

                // Return the auction ID
                return auctionId;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
            finally
            {
                semaphoreSlim.Release();
            }
        }

        public async Task<Guid> OpenAuction(Guid id)
        {
            try
            {
                var auction = await GetAuction(id);
                if (auction == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }

                // Check if the auction is approved
                if (auction.Status != AuctionStatus.Approved)
                {
                    throw new AppException(ErrorMessage.AuctionNotApproved);
                }

                auction.Status = AuctionStatus.OnGoing;
                auction.DateUpdate = DateTime.UtcNow;

                _dataContext.Auctions.Update(auction);
                await _dataContext.SaveChangesAsync();

                return auction.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<Guid> CloseAuction(Guid id)
        {
            try
            {
                var auction = await GetAuction(id);
                if (auction == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }

                // Check if the auction is ongoing
                if (auction.Status != AuctionStatus.OnGoing)
                {
                    throw new AppException(ErrorMessage.AuctionNotOnGoing);
                }

                auction.Status = AuctionStatus.Completed;
                auction.DateUpdate = DateTime.UtcNow;

                _dataContext.Auctions.Update(auction);
                await _dataContext.SaveChangesAsync();

                return auction.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        // private method

        //private void SearchByKeyWord(ref IQueryable<Auction> auctions, string? keyword)
        //{
        //    if (!auctions.Any() || string.IsNullOrWhiteSpace(keyword))
        //        return;
        //    auctions = auctions.Where(o => o.Name.ToLower().Contains(keyword.Trim().ToLower()) || o.AuctionName.ToLower().Contains(keyword.Trim().ToLower()));
        //}
        private async Task<Auction> GetAuction(Guid id)
        {
            try
            {
                var data = await _dataContext
                    .Auctions
                    .Where(x => !x.IsDeleted && x.Id == id)
                    .SingleOrDefaultAsync();
                if (data == null) throw new AppException(ErrorMessage.IdNotExist);
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }
    }
}
