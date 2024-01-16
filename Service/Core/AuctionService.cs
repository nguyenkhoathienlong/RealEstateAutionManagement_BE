using AutoMapper;
using Data.EFCore;
using Data.Entities;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Service.Utilities;

namespace Service.Core
{
    public interface IAuctionService
    {
        Task<PagingModel<AuctionViewModel>> GetAll(AuctionQueryModel query);
        Task<AuctionViewModel> GetById(Guid id);
        Task<Guid> Create(AuctionCreateModel auctionCreateModel);
        Task<Guid> Update(Guid id, AuctionUpdateModel model);
        Task<Guid> Delete(Guid id);

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
