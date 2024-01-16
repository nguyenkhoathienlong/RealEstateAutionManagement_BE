using AutoMapper;
using Data.EFCore;
using Data.Entities;
using Data.Enum;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RealEstateAuctionManagement.Extensions;
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
        Task<Guid> CreateAuctionRequest(AuctionCreateModel auctionCreateModel, string userId);
    }

    public class AuctionService : IAuctionService
    {
        private readonly DataContext _dataContext;
        private ISortHelpers<Auction> _sortHelper;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuctionService(DataContext dataContext, ISortHelpers<Auction> sortHelper, IMapper mapper, IConfiguration configuration)
        {
            _dataContext = dataContext;
            _sortHelper = sortHelper;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<Guid> CreateAuctionRequest(AuctionCreateModel auctionCreateModel, string userId)
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

                // Check if the real estate is already in an auction
                var existingAuction = await _dataContext.Auctions
                    .Where(x => x.RealEstateId == auctionCreateModel.RealEstateId && x.Status != AuctionStatus.Rejected && x.Status != AuctionStatus.Failed)
                    .FirstOrDefaultAsync();
                if (existingAuction != null)
                {
                    throw new AppException(ErrorMessage.RealEstateAlreadyInAuction);
                }

                // Check if the start date is at least 7 days from now
                if (auctionCreateModel.StartDate < DateTime.Now.AddDays(7))
                {
                    throw new AppException(ErrorMessage.StartDateTooEarly);
                }

                // Check if the end date is the same as the start date
                if (auctionCreateModel.EndDate.Date != auctionCreateModel.StartDate.Date)
                {
                    throw new AppException(ErrorMessage.EndDateNotSameAsStartDate);
                }

                // Check if the end time is later than the start time
                if (auctionCreateModel.EndDate.TimeOfDay <= auctionCreateModel.StartDate.TimeOfDay)
                {
                    throw new AppException(ErrorMessage.EndTimeNotLaterThanStartTime);
                }

                var auction = _mapper.Map<AuctionCreateModel, Auction>(auctionCreateModel);
                
                // Get percent from settings
                var registrationFeePercent = float.Parse(_dataContext.Settings.FirstOrDefault(x => x.Key == "REGISTRATION_FEE_PERCENT")?.Value ?? "0") / 100;
                var depositPercent = float.Parse(_dataContext.Settings.FirstOrDefault(x => x.Key == "DEPOSIT_PERCENT")?.Value ?? "0") / 100;

                auction.CreateByUserId = new Guid(userId);
                auction.Status = AuctionStatus.Pending;
                auction.RegistrationFee = auction.StartingPrice * registrationFeePercent;
                auction.Deposit = auction.StartingPrice * depositPercent;

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
    }
}
