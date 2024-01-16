using AutoMapper;
using Data.EFCore;
using Data.Entities;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Service.Utilities;

namespace Service.Core
{
    public interface ISettingService
    {
        Task<PagingModel<SettingViewModel>> GetAll(SettingQueryModel query);
        Task<SettingViewModel> GetById(Guid id);
        Task<Guid> Create(SettingCreateModel model);
        Task<Guid> Update(Guid id, SettingUpdateModel model);
        Task<Guid> Delete(Guid id);

    }
    public class SettingService : ISettingService
    {
        private readonly DataContext _dataContext;
        private ISortHelpers<Setting> _sortHelper;
        private readonly IMapper _mapper;

        public SettingService(DataContext dataContext, ISortHelpers<Setting> sortHelper, IMapper mapper)
        {
            _dataContext = dataContext;
            _sortHelper = sortHelper;
            _mapper = mapper;
        }

        public async Task<Guid> Create(SettingCreateModel model)
        {
            try
            {
                var existedSetting = await _dataContext.Settings
                    .Where(x => !x.IsDeleted)
                    .FirstOrDefaultAsync();
                var data = _mapper.Map<SettingCreateModel, Setting>(model);
                await _dataContext.Settings.AddAsync(data);
                await _dataContext.SaveChangesAsync();
                return data.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<PagingModel<SettingViewModel>> GetAll(SettingQueryModel query)
        {
            try
            {
                var queryData = _dataContext.Settings
                .Where(x => !x.IsDeleted);

                var sortData = _sortHelper.ApplySort(queryData, query.OrderBy!);

                var data = await sortData.ToPagedListAsync(query.PageIndex, query.PageSize);

                var pagingData = new PagingModel<SettingViewModel>()
                {
                    PageIndex = data.CurrentPage,
                    PageSize = data.PageSize,
                    TotalCount = data.TotalCount,
                    pagingData = _mapper.Map<List<Setting>, List<SettingViewModel>>(data)
                };
                return pagingData;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<SettingViewModel> GetById(Guid id)
        {
            try
            {
                var data = await GetSetting(id);
                if (data == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }
                return _mapper.Map<Setting, SettingViewModel>(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<Guid> Update(Guid id, SettingUpdateModel model)
        {
            try
            {
                var checkExistSetting = await GetSetting(id);
                if (checkExistSetting == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }
                var updateData = _mapper.Map(model, checkExistSetting);
                _dataContext.Settings.Update(updateData);
                await _dataContext.SaveChangesAsync();
                return checkExistSetting.Id;
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
                var checkExistSetting = await GetSetting(id);
                if (checkExistSetting == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }
                checkExistSetting.IsDeleted = true;
                _dataContext.Settings.Update(checkExistSetting);
                await _dataContext.SaveChangesAsync();
                return checkExistSetting.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        // private method

        //private void SearchByKeyWord(ref IQueryable<Setting> Settings, string? keyword)
        //{
        //    if (!Settings.Any() || string.IsNullOrWhiteSpace(keyword))
        //        return;
        //    Settings = Settings.Where(o => o.Name.ToLower().Contains(keyword.Trim().ToLower()) || o.SettingName.ToLower().Contains(keyword.Trim().ToLower()));
        //}
        private async Task<Setting> GetSetting(Guid id)
        {
            try
            {
                var data = await _dataContext
                    .Settings
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
