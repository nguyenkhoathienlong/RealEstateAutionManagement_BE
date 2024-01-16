using AutoMapper;
using Data.EFCore;
using Data.Entities;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Service.Utilities;

namespace Service.Core
{
    public interface IRealEstateService
    {
        Task<PagingModel<RealEstateViewModel>> GetAll(RealEstateQueryModel query);
        Task<RealEstateViewModel> GetById(Guid id);
        Task<Guid> Create(RealEstateCreateModel model);
        Task<Guid> Update(Guid id, RealEstateUpdateModel model);
        Task<Guid> Delete(Guid id);

    }
    public class RealEstateService : IRealEstateService
    {
        private readonly DataContext _dataContext;
        private ISortHelpers<RealEstate> _sortHelper;
        private readonly IMapper _mapper;

        public RealEstateService(DataContext dataContext, ISortHelpers<RealEstate> sortHelper, IMapper mapper)
        {
            _dataContext = dataContext;
            _sortHelper = sortHelper;
            _mapper = mapper;
        }

        public async Task<Guid> Create(RealEstateCreateModel model)
        {
            try
            {
                var existedRealEstate = await _dataContext.RealEstates
                    .Where(x => !x.IsDeleted)
                    .FirstOrDefaultAsync();
                var data = _mapper.Map<RealEstateCreateModel, RealEstate>(model);
                await _dataContext.RealEstates.AddAsync(data);
                await _dataContext.SaveChangesAsync();
                return data.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<PagingModel<RealEstateViewModel>> GetAll(RealEstateQueryModel query)
        {
            try
            {
                var queryData = _dataContext.RealEstates
                .Where(x => !x.IsDeleted);

                var sortData = _sortHelper.ApplySort(queryData, query.OrderBy!);

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

        public async Task<RealEstateViewModel> GetById(Guid id)
        {
            try
            {
                var data = await GetRealEstate(id);
                if (data == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }
                return _mapper.Map<RealEstate, RealEstateViewModel>(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<Guid> Update(Guid id, RealEstateUpdateModel model)
        {
            try
            {
                var checkExistRealEstate = await GetRealEstate(id);
                if (checkExistRealEstate == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }
                var updateData = _mapper.Map(model, checkExistRealEstate);
                _dataContext.RealEstates.Update(updateData);
                await _dataContext.SaveChangesAsync();
                return checkExistRealEstate.Id;
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
                var checkExistRealEstate = await GetRealEstate(id);
                if (checkExistRealEstate == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
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
