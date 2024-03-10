using AutoMapper;
using Data.EFCore;
using Data.Entities;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Service.Utilities;

namespace Service.Core
{
    public interface IFeedbackService
    {
        Task<PagingModel<FeedbackViewModel>> GetAll(FeedbackQueryModel query);
        Task<FeedbackViewModel> GetById(Guid id);
        Task<Guid> Create(FeedbackCreateModel model);
        Task<Guid> Update(Guid id, FeedbackUpdateModel model);
        Task<Guid> Delete(Guid id);

    }
    public class FeedbackService : IFeedbackService
    {
        private readonly DataContext _dataContext;
        private ISortHelpers<Feedback> _sortHelper;
        private readonly IMapper _mapper;

        public FeedbackService(DataContext dataContext, ISortHelpers<Feedback> sortHelper, IMapper mapper)
        {
            _dataContext = dataContext;
            _sortHelper = sortHelper;
            _mapper = mapper;
        }

        public async Task<Guid> Create(FeedbackCreateModel model)
        {
            try
            {
                var existedFeedback = await _dataContext.Feedbacks
                    .Where(x => !x.IsDeleted)
                    .FirstOrDefaultAsync();
                var data = _mapper.Map<FeedbackCreateModel, Feedback>(model);
                await _dataContext.Feedbacks.AddAsync(data);
                await _dataContext.SaveChangesAsync();
                return data.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<PagingModel<FeedbackViewModel>> GetAll(FeedbackQueryModel query)
        {
            try
            {
                var queryData = _dataContext.Feedbacks
                .Where(x => !x.IsDeleted);

                var sortData = _sortHelper.ApplySort(queryData, query.OrderBy!);

                var data = await sortData.ToPagedListAsync(query.PageIndex, query.PageSize);

                var pagingData = new PagingModel<FeedbackViewModel>()
                {
                    PageIndex = data.CurrentPage,
                    PageSize = data.PageSize,
                    TotalCount = data.TotalCount,
                    pagingData = _mapper.Map<List<Feedback>, List<FeedbackViewModel>>(data)
                };
                return pagingData;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<FeedbackViewModel> GetById(Guid id)
        {
            try
            {
                var data = await GetFeedback(id);
                if (data == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }
                return _mapper.Map<Feedback, FeedbackViewModel>(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<Guid> Update(Guid id, FeedbackUpdateModel model)
        {
            try
            {
                var checkExistFeedback = await GetFeedback(id);
                if (checkExistFeedback == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }
                var updateData = _mapper.Map(model, checkExistFeedback);
                _dataContext.Feedbacks.Update(updateData);
                await _dataContext.SaveChangesAsync();
                return checkExistFeedback.Id;
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
                var checkExistFeedback = await GetFeedback(id);
                if (checkExistFeedback == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }
                checkExistFeedback.IsDeleted = true;
                _dataContext.Feedbacks.Update(checkExistFeedback);
                await _dataContext.SaveChangesAsync();
                return checkExistFeedback.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        // private method

        //private void SearchByKeyWord(ref IQueryable<Feedback> Feedbacks, string? keyword)
        //{
        //    if (!Feedbacks.Any() || string.IsNullOrWhiteSpace(keyword))
        //        return;
        //    Feedbacks = Feedbacks.Where(o => o.Name.ToLower().Contains(keyword.Trim().ToLower()) || o.FeedbackName.ToLower().Contains(keyword.Trim().ToLower()));
        //}
        private async Task<Feedback> GetFeedback(Guid id)
        {
            try
            {
                var data = await _dataContext
                    .Feedbacks
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
