using AutoMapper;
using Data.EFCore;
using Data.Entities;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Service.Utilities;

namespace Service.Core
{
    public interface INotificationService
    {
        Task<PagingModel<NotificationViewModel>> GetAll(NotificationQueryModel query);
        Task<NotificationViewModel> GetById(Guid id);
        Task<Guid> Create(NotificationCreateModel model);
        Task<Guid> Update(Guid id, NotificationUpdateModel model);
        Task<Guid> Delete(Guid id);

    }
    public class NotificationService : INotificationService
    {
        private readonly DataContext _dataContext;
        private ISortHelpers<Notification> _sortHelper;
        private readonly IMapper _mapper;

        public NotificationService(DataContext dataContext, ISortHelpers<Notification> sortHelper, IMapper mapper)
        {
            _dataContext = dataContext;
            _sortHelper = sortHelper;
            _mapper = mapper;
        }

        public async Task<Guid> Create(NotificationCreateModel model)
        {
            try
            {
                var existedNotification = await _dataContext.Notifications
                    .Where(x => !x.IsDeleted)
                    .FirstOrDefaultAsync();
                var data = _mapper.Map<NotificationCreateModel, Notification>(model);
                await _dataContext.Notifications.AddAsync(data);
                await _dataContext.SaveChangesAsync();
                return data.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<PagingModel<NotificationViewModel>> GetAll(NotificationQueryModel query)
        {
            try
            {
                var queryData = _dataContext.Notifications
                .Where(x => !x.IsDeleted);

                var sortData = _sortHelper.ApplySort(queryData, query.OrderBy!);

                var data = await sortData.ToPagedListAsync(query.PageIndex, query.PageSize);

                var pagingData = new PagingModel<NotificationViewModel>()
                {
                    PageIndex = data.CurrentPage,
                    PageSize = data.PageSize,
                    TotalCount = data.TotalCount,
                    pagingData = _mapper.Map<List<Notification>, List<NotificationViewModel>>(data)
                };
                return pagingData;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<NotificationViewModel> GetById(Guid id)
        {
            try
            {
                var data = await GetNotification(id);
                if (data == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }
                return _mapper.Map<Notification, NotificationViewModel>(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<Guid> Update(Guid id, NotificationUpdateModel model)
        {
            try
            {
                var checkExistNotification = await GetNotification(id);
                if (checkExistNotification == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }
                var updateData = _mapper.Map(model, checkExistNotification);
                _dataContext.Notifications.Update(updateData);
                await _dataContext.SaveChangesAsync();
                return checkExistNotification.Id;
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
                var checkExistNotification = await GetNotification(id);
                if (checkExistNotification == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }
                checkExistNotification.IsDeleted = true;
                _dataContext.Notifications.Update(checkExistNotification);
                await _dataContext.SaveChangesAsync();
                return checkExistNotification.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        // private method

        //private void SearchByKeyWord(ref IQueryable<Notification> Notifications, string? keyword)
        //{
        //    if (!Notifications.Any() || string.IsNullOrWhiteSpace(keyword))
        //        return;
        //    Notifications = Notifications.Where(o => o.Name.ToLower().Contains(keyword.Trim().ToLower()) || o.NotificationName.ToLower().Contains(keyword.Trim().ToLower()));
        //}
        private async Task<Notification> GetNotification(Guid id)
        {
            try
            {
                var data = await _dataContext
                    .Notifications
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
