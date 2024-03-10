using AutoMapper;
using Data.EFCore;
using Data.Entities;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Service.Utilities;

namespace Service.Core
{
    public interface ITransactionService
    {
        Task<PagingModel<TransactionViewModel>> GetAll(TransactionQueryModel query);
        Task<TransactionViewModel> GetById(Guid id);
        Task<Guid> Create(TransactionCreateModel model);
        Task<Guid> Update(Guid id, TransactionUpdateModel model);
        Task<Guid> Delete(Guid id);

    }
    public class TransactionService : ITransactionService
    {
        private readonly DataContext _dataContext;
        private ISortHelpers<Transaction> _sortHelper;
        private readonly IMapper _mapper;

        public TransactionService(DataContext dataContext, ISortHelpers<Transaction> sortHelper, IMapper mapper)
        {
            _dataContext = dataContext;
            _sortHelper = sortHelper;
            _mapper = mapper;
        }

        public async Task<Guid> Create(TransactionCreateModel model)
        {
            try
            {
                var existedTransaction = await _dataContext.Transaction
                    .Where(x => !x.IsDeleted)
                    .FirstOrDefaultAsync();
                var data = _mapper.Map<TransactionCreateModel, Transaction>(model);
                await _dataContext.Transaction.AddAsync(data);
                await _dataContext.SaveChangesAsync();
                return data.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<PagingModel<TransactionViewModel>> GetAll(TransactionQueryModel query)
        {
            try
            {
                var queryData = _dataContext.Transaction
                .Where(x => !x.IsDeleted);

                var sortData = _sortHelper.ApplySort(queryData, query.OrderBy!);

                var data = await sortData.ToPagedListAsync(query.PageIndex, query.PageSize);

                var pagingData = new PagingModel<TransactionViewModel>()
                {
                    PageIndex = data.CurrentPage,
                    PageSize = data.PageSize,
                    TotalCount = data.TotalCount,
                    pagingData = _mapper.Map<List<Transaction>, List<TransactionViewModel>>(data)
                };
                return pagingData;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<TransactionViewModel> GetById(Guid id)
        {
            try
            {
                var data = await GetTransaction(id);
                if (data == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }
                return _mapper.Map<Transaction, TransactionViewModel>(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<Guid> Update(Guid id, TransactionUpdateModel model)
        {
            try
            {
                var checkExistTransaction = await GetTransaction(id);
                if (checkExistTransaction == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }
                var updateData = _mapper.Map(model, checkExistTransaction);
                _dataContext.Transaction.Update(updateData);
                await _dataContext.SaveChangesAsync();
                return checkExistTransaction.Id;
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
                var checkExistTransaction = await GetTransaction(id);
                if (checkExistTransaction == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }
                checkExistTransaction.IsDeleted = true;
                _dataContext.Transaction.Update(checkExistTransaction);
                await _dataContext.SaveChangesAsync();
                return checkExistTransaction.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        // private method

        //private void SearchByKeyWord(ref IQueryable<Transaction> Transactions, string? keyword)
        //{
        //    if (!Transactions.Any() || string.IsNullOrWhiteSpace(keyword))
        //        return;
        //    Transactions = Transactions.Where(o => o.Name.ToLower().Contains(keyword.Trim().ToLower()) || o.TransactionName.ToLower().Contains(keyword.Trim().ToLower()));
        //}
        private async Task<Transaction> GetTransaction(Guid id)
        {
            try
            {
                var data = await _dataContext
                    .Transaction
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
