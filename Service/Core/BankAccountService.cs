using AutoMapper;
using Data.EFCore;
using Data.Entities;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Service.Utilities;

namespace Service.Core
{
    public interface IBankAccountService
    {
        Task<PagingModel<BankAccountViewModel>> GetAll(BankAccountQueryModel query);
        Task<BankAccountViewModel> GetById(Guid id);
        Task<Guid> Create(BankAccountCreateModel BankAccountCreateModel);
        Task<Guid> Update(Guid id, BankAccountUpdateModel model);
        Task<Guid> Delete(Guid id);

    }
    public class BankAccountService : IBankAccountService
    {
        private readonly DataContext _dataContext;
        private ISortHelpers<BankAccount> _sortHelper;
        private readonly IMapper _mapper;

        public BankAccountService(DataContext dataContext, ISortHelpers<BankAccount> sortHelper, IMapper mapper)
        {
            _dataContext = dataContext;
            _sortHelper = sortHelper;
            _mapper = mapper;
        }

        public async Task<Guid> Create(BankAccountCreateModel model)
        {
            try
            {
                var existedBankAccount = await _dataContext.BankAccounts
                    .Where(x => !x.IsDeleted)
                    .FirstOrDefaultAsync();
                var data = _mapper.Map<BankAccountCreateModel, BankAccount>(model);
                await _dataContext.BankAccounts.AddAsync(data);
                await _dataContext.SaveChangesAsync();
                return data.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<PagingModel<BankAccountViewModel>> GetAll(BankAccountQueryModel query)
        {
            try
            {
                var queryData = _dataContext.BankAccounts
                .Where(x => !x.IsDeleted);

                var sortData = _sortHelper.ApplySort(queryData, query.OrderBy!);

                var data = await sortData.ToPagedListAsync(query.PageIndex, query.PageSize);

                var pagingData = new PagingModel<BankAccountViewModel>()
                {
                    PageIndex = data.CurrentPage,
                    PageSize = data.PageSize,
                    TotalCount = data.TotalCount,
                    pagingData = _mapper.Map<List<BankAccount>, List<BankAccountViewModel>>(data)
                };
                return pagingData;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<BankAccountViewModel> GetById(Guid id)
        {
            try
            {
                var data = await GetBankAccount(id);
                if (data == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }
                return _mapper.Map<BankAccount, BankAccountViewModel>(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<Guid> Update(Guid id, BankAccountUpdateModel model)
        {
            try
            {
                var checkExistBankAccount = await GetBankAccount(id);
                if (checkExistBankAccount == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }
                var updateData = _mapper.Map(model, checkExistBankAccount);
                _dataContext.BankAccounts.Update(updateData);
                await _dataContext.SaveChangesAsync();
                return checkExistBankAccount.Id;
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
                var checkExistBankAccount = await GetBankAccount(id);
                if (checkExistBankAccount == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }
                checkExistBankAccount.IsDeleted = true;
                _dataContext.BankAccounts.Update(checkExistBankAccount);
                await _dataContext.SaveChangesAsync();
                return checkExistBankAccount.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        // private method

        //private void SearchByKeyWord(ref IQueryable<BankAccount> BankAccounts, string? keyword)
        //{
        //    if (!BankAccounts.Any() || string.IsNullOrWhiteSpace(keyword))
        //        return;
        //    BankAccounts = BankAccounts.Where(o => o.Name.ToLower().Contains(keyword.Trim().ToLower()) || o.BankAccountName.ToLower().Contains(keyword.Trim().ToLower()));
        //}
        private async Task<BankAccount> GetBankAccount(Guid id)
        {
            try
            {
                var data = await _dataContext
                    .BankAccounts
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
