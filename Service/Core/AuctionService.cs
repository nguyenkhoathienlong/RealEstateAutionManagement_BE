﻿using AutoMapper;
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
        Task<PagingModel<AuctionViewModel>> GetAll(AuctionQueryModel query);
        Task<AuctionViewModel> GetById(Guid id);
        Task<Guid> Create(AuctionCreateModel auctionCreateModel);
        Task<Guid> Update(Guid id, AuctionUpdateModel model);
        Task<Guid> Delete(Guid id);
        Task<Guid> CreateAuctionRequest(AuctionCreateRequestModel auctionCreateModel, string userId);
    }

    public class AuctionService : IAuctionService
    {
        private readonly DataContext _dataContext;
        private ISortHelpers<Auction> _sortHelper;
        private readonly IMapper _mapper;

        public AuctionService(DataContext dataContext, ISortHelpers<Auction> sortHelper, IMapper mapper)
        {
            _dataContext = dataContext;
            _sortHelper = sortHelper;
            _mapper = mapper;
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

        public async Task<PagingModel<AuctionViewModel>> GetAll(AuctionQueryModel query)
        {
            try
            {
                var queryData = _dataContext.Auctions
                .Where(x => !x.IsDeleted);

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

        public async Task<Guid> Update(Guid id, AuctionUpdateModel model)
        {
            try
            {
                var checkExistAuction = await GetAuction(id);
                if (checkExistAuction == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
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

        public async Task<Guid> Delete(Guid id)
        {
            try
            {
                var checkExistAuction = await GetAuction(id);
                if (checkExistAuction == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
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
                auction.Status = AuctionStatus.Pending;
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