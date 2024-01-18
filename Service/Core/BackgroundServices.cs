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
            var auction = await _dataContext.Auctions.FindAsync(auctionId);
            if (auction != null && auction.Status == AuctionStatus.OnGoing)
            {
                auction.Status = AuctionStatus.Completed;
                _dataContext.Auctions.Update(auction);
                await _dataContext.SaveChangesAsync();
                _logger.LogInformation($"Auction {auctionId} has been closed.");
            }
            else
            {
                _logger.LogWarning($"Cannot close auction {auctionId}. The auction might not exist or not being opened for bidding.");
            }
        }
    }
}
