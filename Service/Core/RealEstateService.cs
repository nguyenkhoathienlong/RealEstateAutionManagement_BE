using AutoMapper;
using Data.EFCore;
using Data.Entities;
using Data.Enum;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Service.Utilities;

namespace Service.Core
{
    public interface IRealEstateService
    {
        Task<PagingModel<RealEstateViewModel>> GetAll(RealEstateQueryModel query, string userId);
        Task<RealEstateViewModel> GetById(Guid id, string userId);
        Task<Guid> Create(RealEstateCreateModel model, string userId);
        Task<Guid> Update(Guid id, RealEstateUpdateModel model, string userId);
        Task<Guid> Delete(Guid id, string userId);
        Task<Guid> ApproveRealEstate(Guid id, ApproveRealEstateModel model, string approvedById);
    }
    public class RealEstateService : IRealEstateService
    {
        private readonly DataContext _dataContext;
        private ISortHelpers<RealEstate> _sortHelper;
        private readonly IMapper _mapper;
        private readonly IFirebaseStorageService _firebaseStorageService;

        public RealEstateService(DataContext dataContext, ISortHelpers<RealEstate> sortHelper, IMapper mapper, IFirebaseStorageService firebaseStorageService)
        {
            _dataContext = dataContext;
            _sortHelper = sortHelper;
            _mapper = mapper;
            _firebaseStorageService = firebaseStorageService;
        }

        public async Task<Guid> Create(RealEstateCreateModel model, string userId)
        {
            try
            {
                //var existedRealEstate = await _dataContext.RealEstates
                //    .Where(x => !x.IsDeleted)
                //    .FirstOrDefaultAsync();

                // Check if the category exists
                var categoryExist = await _dataContext.Categories
                    .FirstOrDefaultAsync(c => c.Id == model.CategoryId);
                if (categoryExist == null)
                {
                    throw new AppException(ErrorMessage.CategoryNotExist);
                }

                var user = await _dataContext.Users
                    .FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == new Guid(userId));
                if (user == null)
                {
                    throw new AppException(ErrorMessage.UserNameDoNotExist);
                }

                var data = _mapper.Map<RealEstateCreateModel, RealEstate>(model);

                data.ApproveTime = null;
                data.Status = RealEstateStatus.Pending;
                data.UserId = new Guid(userId);

                await _dataContext.RealEstates.AddAsync(data);
                await _dataContext.SaveChangesAsync();

                // Add images to the RealEstateImage table
                if (model.Images != null)
                {
                    foreach (var image in model.Images)
                    {
                        string path = data.Id.ToString() + "/RealEstate";
                        string filename = Guid.NewGuid().ToString() + image.FileName;
                        string imageUrl = await _firebaseStorageService.UploadFileAsync(image, path, filename);

                        var realEstateImage = new RealEstateImage
                        {
                            Image = imageUrl,
                            RealEstateId = data.Id
                        };
                        await _dataContext.RealEstateImages.AddAsync(realEstateImage);
                    } 
                }
                await _dataContext.SaveChangesAsync();

                return data.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<PagingModel<RealEstateViewModel>> GetAll(RealEstateQueryModel query, string userId)
        {
            try
            {
                var user = await _dataContext.Users
                    .FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == new Guid(userId));
                if (user == null)
                {
                    throw new AppException(ErrorMessage.UserNameDoNotExist);
                }

                IQueryable<RealEstate> queryData;

                if (user.Role == Role.Member)
                {
                    queryData = _dataContext.RealEstates
                        .Include(x=>x.RealEstateImages)
                        .Where(x => !x.IsDeleted && x.UserId == user.Id);
                }
                else
                {
                    queryData = _dataContext.RealEstates
                        .Include(x => x.RealEstateImages)
                        .Where(x => !x.IsDeleted);
                }

                var sortData = _sortHelper.ApplySort(queryData, "Status");

                var data = await sortData.ToPagedListAsync(query.PageIndex, query.PageSize);

                var pagingData = new PagingModel<RealEstateViewModel>()
                {
                    PageIndex = data.CurrentPage,
                    PageSize = data.PageSize,
                    TotalCount = data.TotalCount,
                    pagingData = _mapper.Map<List<RealEstate>, List<RealEstateViewModel>>(data)
                };
                return pagingData;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<RealEstateViewModel> GetById(Guid id, string userId)
        {
            try
            {
                var user = await _dataContext.Users
                    .FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == new Guid(userId));
                if (user == null)
                {
                    throw new AppException(ErrorMessage.UserNameDoNotExist);
                }

                var data = await GetRealEstate(id);
                if (data == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }

                // Check if the user is a member and if they own the real estate
                if (user.Role == Role.Member && data.UserId != new Guid(userId))
                {
                    throw new AppException(ErrorMessage.RealEstateNotExist);
                }

                return _mapper.Map<RealEstate, RealEstateViewModel>(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<Guid> Update(Guid id, RealEstateUpdateModel model, string userId)
        {
            try
            {
                var existRealEstate = await GetRealEstate(id);
                if (existRealEstate == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }

                // Check if the category exists
                var categoryExist = await _dataContext.Categories
                    .FirstOrDefaultAsync(c => c.Id == model.CategoryId);
                if (categoryExist == null)
                {
                    throw new AppException(ErrorMessage.CategoryNotExist);
                }

                var user = await _dataContext.Users
                    .FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == new Guid(userId));
                if (user == null)
                {
                    throw new AppException(ErrorMessage.UserNameDoNotExist);
                }

                // Check if the user is a member and if they own the real estate
                if (user.Role == Role.Member && existRealEstate.UserId != new Guid(userId))
                {
                    throw new AppException(ErrorMessage.RealEstateNotExist);
                }

                // Check if the real estate is sold or rejected
                if (existRealEstate.Status == RealEstateStatus.Sold || existRealEstate.Status == RealEstateStatus.Rejected)
                {
                    throw new AppException(ErrorMessage.RealEstateSoldOrRejected);
                }

                // Check if the real estate is either not in an auction or in auction with status pending only
                var existingAuction = await _dataContext.Auctions
                    .Where(x => !x.IsDeleted && x.RealEstateId == id && x.Status != AuctionStatus.Pending)
                    .FirstOrDefaultAsync();
                if (existingAuction != null)
                {
                    throw new AppException(ErrorMessage.RealEstateAlreadyInAuction);
                }

                var data = _mapper.Map(model, existRealEstate);
                _dataContext.RealEstates.Update(data);
                await _dataContext.SaveChangesAsync();
                return existRealEstate.Id;
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
                var checkExistRealEstate = await GetRealEstate(id);
                if (checkExistRealEstate == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }

                var user = await _dataContext.Users
                    .FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == new Guid(userId));
                if (user == null)
                {
                    throw new AppException(ErrorMessage.UserNameDoNotExist);
                }

                // Check if the user is a member and if they own the real estate
                if (user.Role == Role.Member && checkExistRealEstate.UserId != new Guid(userId))
                {
                    throw new AppException(ErrorMessage.RealEstateNotExist);
                }

                // Check if the real estate is already in an auction, regardless of auction status
                var existingAuction = await _dataContext.Auctions
                    .Where(x => !x.IsDeleted && x.RealEstateId == id)
                    .FirstOrDefaultAsync();
                if (existingAuction != null)
                {
                    throw new AppException(ErrorMessage.RealEstateAlreadyInAuction);
                }

                checkExistRealEstate.IsDeleted = true;
                _dataContext.RealEstates.Update(checkExistRealEstate);
                await _dataContext.SaveChangesAsync();
                return checkExistRealEstate.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<Guid> ApproveRealEstate(Guid id, ApproveRealEstateModel model, string approvedById)
        {
            try
            {
                var realEstate = await _dataContext.RealEstates
                    .FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id);
                if (realEstate == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }

                // Check if the real estate is pending
                if (realEstate.Status != RealEstateStatus.Pending)
                {
                    throw new AppException(ErrorMessage.RealEstateNotPending);
                }

                if (model.IsApproved)
                {
                    realEstate.Status = RealEstateStatus.Approved;
                    realEstate.ApproveByUserId = new Guid(approvedById);
                    realEstate.ApproveTime = DateTime.UtcNow;
                }
                else
                {
                    realEstate.Status = RealEstateStatus.Rejected;
                }

                _dataContext.RealEstates.Update(realEstate);
                await _dataContext.SaveChangesAsync();

                var notification = new Notification
                {
                    Title = model.IsApproved ? "Bất động sản đã được duyệt" : "Bất động sản bị từ chối",
                    Description = model.IsApproved ? $"Bất động sản {realEstate.Name} của bạn đã được duyệt."
                        : $"Bất động sản {realEstate.Name} của bạn đã bị từ chối do thông tin cung cấp chưa phù hợp.",
                    UserId = realEstate.UserId
                };

                _dataContext.Notifications.Add(notification);
                await _dataContext.SaveChangesAsync();

                return realEstate.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        // private method

        //private void SearchByKeyWord(ref IQueryable<RealEstate> RealEstates, string? keyword)
        //{
        //    if (!RealEstates.Any() || string.IsNullOrWhiteSpace(keyword))
        //        return;
        //    RealEstates = RealEstates.Where(o => o.Name.ToLower().Contains(keyword.Trim().ToLower()) || o.RealEstateName.ToLower().Contains(keyword.Trim().ToLower()));
        //}
        private async Task<RealEstate> GetRealEstate(Guid id)
        {
            try
            {
                var data = await _dataContext
                    .RealEstates
                    .Include(x => x.RealEstateImages)
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
