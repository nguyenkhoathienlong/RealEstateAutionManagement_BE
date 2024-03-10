using AutoMapper;
using Data.EFCore;
using Data.Entities;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Core
{
    public interface IRealEstateImageService
    {
        Task<PagingModel<RealEstateImageViewModel>> GetAll(RealEstateImageQueryModel query);
        Task<RealEstateImageViewModel> GetById(Guid id);
        Task<Guid> Create(RealEstateImageCreateModel model);
        Task<Guid> Update(Guid id, RealEstateImageUpdateModel model);
        Task<Guid> Delete(Guid id);

    }
    public class RealEstateImageService : IRealEstateImageService
    {
        private readonly DataContext _dataContext;
        private ISortHelpers<RealEstateImage> _sortHelper;
        private readonly IMapper _mapper;

        public RealEstateImageService(DataContext dataContext, ISortHelpers<RealEstateImage> sortHelper, IMapper mapper)
        {
            _dataContext = dataContext;
            _sortHelper = sortHelper;
            _mapper = mapper;
        }

        public async Task<Guid> Create(RealEstateImageCreateModel model)
        {
            try
            {
                var existedRealEstateImage = await _dataContext.RealEstateImages
                    .Where(x => !x.IsDeleted)
                    .FirstOrDefaultAsync();
                var data = _mapper.Map<RealEstateImageCreateModel, RealEstateImage>(model);
                await _dataContext.RealEstateImages.AddAsync(data);
                await _dataContext.SaveChangesAsync();
                return data.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<PagingModel<RealEstateImageViewModel>> GetAll(RealEstateImageQueryModel query)
        {
            try
            {
                var queryData = _dataContext.RealEstateImages
                .Where(x => !x.IsDeleted);

                var sortData = _sortHelper.ApplySort(queryData, query.OrderBy!);

                var data = await sortData.ToPagedListAsync(query.PageIndex, query.PageSize);

                var pagingData = new PagingModel<RealEstateImageViewModel>()
                {
                    PageIndex = data.CurrentPage,
                    PageSize = data.PageSize,
                    TotalCount = data.TotalCount,
                    pagingData = _mapper.Map<List<RealEstateImage>, List<RealEstateImageViewModel>>(data)
                };
                return pagingData;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<RealEstateImageViewModel> GetById(Guid id)
        {
            try
            {
                var data = await GetRealEstateImage(id);
                if (data == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }
                return _mapper.Map<RealEstateImage, RealEstateImageViewModel>(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<Guid> Update(Guid id, RealEstateImageUpdateModel model)
        {
            try
            {
                var checkExistRealEstateImage = await GetRealEstateImage(id);
                if (checkExistRealEstateImage == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }
                var updateData = _mapper.Map(model, checkExistRealEstateImage);
                _dataContext.RealEstateImages.Update(updateData);
                await _dataContext.SaveChangesAsync();
                return checkExistRealEstateImage.Id;
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
                var checkExistRealEstateImage = await GetRealEstateImage(id);
                if (checkExistRealEstateImage == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }
                checkExistRealEstateImage.IsDeleted = true;
                _dataContext.RealEstateImages.Update(checkExistRealEstateImage);
                await _dataContext.SaveChangesAsync();
                return checkExistRealEstateImage.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        // private method

        //private void SearchByKeyWord(ref IQueryable<RealEstateImage> RealEstateImages, string? keyword)
        //{
        //    if (!RealEstateImages.Any() || string.IsNullOrWhiteSpace(keyword))
        //        return;
        //    RealEstateImages = RealEstateImages.Where(o => o.Name.ToLower().Contains(keyword.Trim().ToLower()) || o.RealEstateImageName.ToLower().Contains(keyword.Trim().ToLower()));
        //}
        private async Task<RealEstateImage> GetRealEstateImage(Guid id)
        {
            try
            {
                var data = await _dataContext
                    .RealEstateImages
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
