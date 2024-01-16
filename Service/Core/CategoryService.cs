using AutoMapper;
using Data.EFCore;
using Data.Entities;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Service.Utilities;

namespace Service.Core
{
    public interface ICategoriesService
    {
        Task<PagingModel<CategoryViewModel>> GetAll(CategoryQueryModel query);
        Task<CategoryViewModel> GetById(Guid id);
        Task<Guid> Create(CategoryCreateModel model);
        Task<Guid> Update(Guid id, CategoryUpdateModel model);
        Task<Guid> Delete(Guid id);

    }
    public class CategoriesService : ICategoriesService
    {
        private readonly DataContext _dataContext;
        private ISortHelpers<Category> _sortHelper;
        private readonly IMapper _mapper;

        public CategoriesService(DataContext dataContext, ISortHelpers<Category> sortHelper, IMapper mapper)
        {
            _dataContext = dataContext;
            _sortHelper = sortHelper;
            _mapper = mapper;
        }

        public async Task<Guid> Create(CategoryCreateModel model)
        {
            try
            {
                var existedCategory = await _dataContext.Categories
                    .Where(x => !x.IsDeleted)
                    .FirstOrDefaultAsync();
                var data = _mapper.Map<CategoryCreateModel, Category>(model);
                await _dataContext.Categories.AddAsync(data);
                await _dataContext.SaveChangesAsync();
                return data.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<PagingModel<CategoryViewModel>> GetAll(CategoryQueryModel query)
        {
            try
            {
                var queryData = _dataContext.Categories
                .Where(x => !x.IsDeleted);

                var sortData = _sortHelper.ApplySort(queryData, query.OrderBy!);

                var data = await sortData.ToPagedListAsync(query.PageIndex, query.PageSize);

                var pagingData = new PagingModel<CategoryViewModel>()
                {
                    PageIndex = data.CurrentPage,
                    PageSize = data.PageSize,
                    TotalCount = data.TotalCount,
                    pagingData = _mapper.Map<List<Category>, List<CategoryViewModel>>(data)
                };
                return pagingData;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<CategoryViewModel> GetById(Guid id)
        {
            try
            {
                var data = await GetCategory(id);
                if (data == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }
                return _mapper.Map<Category, CategoryViewModel>(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<Guid> Update(Guid id, CategoryUpdateModel model)
        {
            try
            {
                var checkExistCategory = await GetCategory(id);
                if (checkExistCategory == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }
                var updateData = _mapper.Map(model, checkExistCategory);
                _dataContext.Categories.Update(updateData);
                await _dataContext.SaveChangesAsync();
                return checkExistCategory.Id;
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
                var checkExistCategory = await GetCategory(id);
                if (checkExistCategory == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }
                checkExistCategory.IsDeleted = true;
                _dataContext.Categories.Update(checkExistCategory);
                await _dataContext.SaveChangesAsync();
                return checkExistCategory.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        // private method

        //private void SearchByKeyWord(ref IQueryable<Category> Categories, string? keyword)
        //{
        //    if (!Categories.Any() || string.IsNullOrWhiteSpace(keyword))
        //        return;
        //    Categories = Categories.Where(o => o.Name.ToLower().Contains(keyword.Trim().ToLower()) || o.CategoryName.ToLower().Contains(keyword.Trim().ToLower()));
        //}
        private async Task<Category> GetCategory(Guid id)
        {
            try
            {
                var data = await _dataContext
                    .Categories
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
