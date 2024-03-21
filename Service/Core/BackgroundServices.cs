using AutoMapper;
using Data.EFCore;
using Data.Entities;
using Data.Enum;
using Hangfire.Server;
using Hangfire;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core
{
    public partial class BackgroundServices
    {
        private readonly DataContext _dataContext;
        private readonly ILogger<BackgroundServices> _logger;
        private readonly IMapper _mapper;

        public BackgroundServices(DataContext dataContext, ILogger<BackgroundServices> logger, IMapper mapper)
        {
            _dataContext = dataContext;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task OpenAuction(Guid auctionId)
        {
            var auction = await _dataContext.Auctions
                .Include(a => a.RealEstates)
                .FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == auctionId);
            if (auction != null && auction.Status == AuctionStatus.Approved)
            {
                // Check if there are at least two registered users
                var registeredUsers = await _dataContext.UserBids.CountAsync(ub => ub.AuctionId == auctionId && ub.IsDeposit);
                if (registeredUsers >= 2)
                {
                    auction.Status = AuctionStatus.OnGoing;
                    _logger.LogInformation($"Auction {auctionId} has been opened for bidding.");
                }
                else
                {
                    auction.Status = AuctionStatus.Failed;
                    _logger.LogWarning($"Cannot open auction {auctionId} for bidding. Not enough registered users.");

                    // Create a notification
                    var notification = new Notification
                    {
                        Title = "Phiên đấu giá thất bại",
                        Description = $"Không thể mở phiên đấu giá tài sản {auction.RealEstates.Name} do số lượng người dùng tham gia không đủ.",
                        UserId = auction.CreateByUserId
                    };
                    _dataContext.Notifications.Add(notification);
                    await _dataContext.SaveChangesAsync();

                    // Cancel close auction job
                    var closeJobId = KeyValueStore.Instance.Get<string>($"CloseAuctionTask_{auction.Id}");
                    if (!string.IsNullOrEmpty(closeJobId))
                    {
                        _logger.LogInformation("Cancelling close auction job for auctionId: {auctionId} as auction has failed", auctionId);
                        BackgroundJob.Delete(closeJobId);
                        KeyValueStore.Instance.Remove($"CloseAuctionTask_{auction.Id}");
                    }
                }

                _dataContext.Auctions.Update(auction);
                await _dataContext.SaveChangesAsync();
            }
            else
            {
                _logger.LogWarning($"Cannot open auction {auctionId} for bidding. The auction might not exist or not being approved.");
            }
        }

        public async Task CloseAuction(Guid auctionId)
        {
            var auction = await _dataContext.Auctions
                .Include(x => x.RealEstates)
                .FirstOrDefaultAsync(x => x.Id == auctionId);
            if (auction != null && auction.Status == AuctionStatus.OnGoing)
            {
                // Check if there is more than one bid
                var bids = await _dataContext.UserBids.CountAsync(ub => ub.AuctionId == auctionId && !ub.IsDeposit);
                if (bids > 1)
                {
                    auction.Status = AuctionStatus.Completed;
                    _logger.LogInformation($"Auction {auctionId} has been closed.");

                    // Find the winner of the auction
                    var highestBid = await _dataContext.UserBids
                        .Where(x => x.AuctionId == auctionId && !x.IsDeposit)
                        .OrderByDescending(x => x.Amount)
                        .FirstOrDefaultAsync();

                    // Create a notification for the winner
                    if (highestBid != null)
                    {
                        var notification = new Notification
                        {
                            Title = "Chúc mừng bạn đã thắng phiên đấu giá",
                            Description = $"Bạn đã thắng phiên đấu giá cho {auction.RealEstates.Name} với giá {highestBid!.Amount}.",
                            UserId = highestBid.UserId
                        };
                        _dataContext.Notifications.Add(notification);
                        await _dataContext.SaveChangesAsync();

                        // Change status of real estate to sold
                        auction.RealEstates.Status = RealEstateStatus.Sold;
                    }
                }
                else
                {
                    auction.Status = AuctionStatus.Failed;
                    _logger.LogWarning($"Auction {auctionId} failed. Not enough bids.");

                    // Create a notification
                    var notification = new Notification
                    {
                        Title = "Phiên đấu giá thất bại",
                        Description = $"Phiên đấu giá cho {auction.RealEstates.Name} đã không thành công do quá ít người trả giá.",
                        UserId = auction.CreateByUserId
                    };
                    _dataContext.Notifications.Add(notification);
                    await _dataContext.SaveChangesAsync();
                }

                auction.DateUpdate = DateTime.UtcNow;
                _dataContext.Auctions.Update(auction);
                await _dataContext.SaveChangesAsync();
            }
            else
            {
                _logger.LogWarning($"Cannot close auction {auctionId}. The auction might not exist or not being opened for bidding.");
            }
        }

        public async Task CheckTransactionStatus(Guid transactionId)
        {
            _logger.LogInformation("Checking transaction status: {TransactionId}", transactionId);
            var transaction = await _dataContext.Transaction.FirstOrDefaultAsync(x => x.Id == transactionId);
            if (transaction is not null)
            {
                if (transaction.Status.Equals(TransactionStatus.Pending))
                {
                    transaction.Status = TransactionStatus.Failed;
                    transaction.DateUpdate = DateTime.UtcNow;
                    await _dataContext.SaveChangesAsync();
                    _logger.LogWarning("Transaction: {TransactionId} Failed due to timeout", transaction.Id);
                }
                else _logger.LogInformation("Transaction: {TransactionId} is completed", transactionId);
            }
            else _logger.LogWarning("Transaction: {TransactionId} is not found", transactionId);
        }
    }
}
