﻿
using Data.EFCore;
using Data.Entities;
using Data.Models;
using Service.Utilities;
using System.Security.Claims;
using RealEstateAuctionManagement.Extensions;
using Data.Enum;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Google.Cloud.Storage.V1;
using Hangfire;

namespace Service.Core
{
    public interface IUserService
    {
        Task<JWTToken> Login(UserRequest model);
        Task<Guid> Register(UserRegisterModel model);
        Task<PagingModel<UserViewModel>> GetAll(UserQueryModel query);
        Task<UserViewModel> GetById(Guid id);
        Task<Guid> Create(UserCreateModel userCreateModel);
        Task<Guid> Update(Guid id, UserUpdateModel model);
        Task<Guid> Delete(Guid id);
        Task<Guid> UpdateProfileImage(string id, IFormFile image);
        Task<Guid> UploadDocument(string id, IdentificationInformation model);
        Task<Guid> ApproveIdentificationDocument(Guid id, ApprovedIdentificationDocument model, string approvedById);

    }
    public class UserService : IUserService
    {
        private readonly DataContext _dataContext;
        private ISortHelpers<User> _sortHelper;
        private readonly IMapper _mapper;
        private readonly IJwtUtils _jwtUtils;
        private readonly IConfiguration _configuration;
        private readonly IFirebaseStorageService _firebaseStorageService;

        public UserService(DataContext dataContext, ISortHelpers<User> sortHelper, IMapper mapper, IConfiguration configuration, IJwtUtils jwtUtils, IFirebaseStorageService firebaseStorageService)
        {
            _dataContext = dataContext;
            _sortHelper = sortHelper;
            _mapper = mapper;
            _configuration = configuration;
            _jwtUtils = jwtUtils;
            _firebaseStorageService = firebaseStorageService;
        }

        public async Task<JWTToken> Login(UserRequest model)
        {
            try
            {
                var user = await _dataContext.Users
                .Where(x => !x.IsDeleted && x.UserName == model.Username)
                .FirstOrDefaultAsync();
                if (user == null)
                {
                    throw new AppException(ErrorMessage.InvalidAccount);
                }
                if (user.Status == UserStatus.Banned)
                {
                    throw new AppException(ErrorMessage.BannedAccount);
                }
                if (!BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                {
                    throw new AppException(ErrorMessage.InvalidAccount);
                }
                var getRole = user.Role;
                var authClaims = new List<Claim>
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim(ClaimTypes.Name, model?.Username ?? ""),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, getRole.ToString())
                };

                var token = _jwtUtils.GenerateToken(authClaims, _configuration.GetSection("JWT").Get<JwtModel>());
                return token;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<Guid> Register(UserRegisterModel model)
        {
            using (var transaction = _dataContext.Database.BeginTransaction())
            {
                try
                {
                    // new user register
                    var existedUser = await _dataContext.Users
                    .Where(x => !x.IsDeleted && x.UserName.Equals(model.UserName) || x.PhoneNumber.Equals(model.PhoneNumber))
                    .FirstOrDefaultAsync();
                    if (existedUser != null)
                    {
                        throw new AppException(ErrorMessage.UserNameExist + " or " + ErrorMessage.PhoneExist);
                    }
                    string passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
                    model.Password = passwordHash;
                    var role = Role.Member;
                    model.Role = role;
                    var dataUser = _mapper.Map<UserRegisterModel, User>(model);
                    await _dataContext.Users.AddAsync(dataUser);
                    await _dataContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return dataUser.Id;

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    await transaction.RollbackAsync();
                    throw new AppException(e.Message);
                }
            }
        }

        public async Task<Guid> Create(UserCreateModel model)
        {
            try
            {
                var existedUser = await _dataContext.Users
                    .Where(x => !x.IsDeleted && x.UserName.Equals(model.UserName) || x.PhoneNumber.Equals(model.PhoneNumber))
                    .FirstOrDefaultAsync();
                if (existedUser != null)
                {
                    throw new AppException(ErrorMessage.UserNameExist + " or " + ErrorMessage.PhoneExist);
                }
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);
                model.Password = passwordHash;
                var data = _mapper.Map<UserCreateModel, User>(model);
                await _dataContext.Users.AddAsync(data);
                await _dataContext.SaveChangesAsync();
                return data.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<PagingModel<UserViewModel>> GetAll(UserQueryModel query)
        {
            try
            {
                var queryData = _dataContext.Users
                .Where(x => !x.IsDeleted);
                SearchByKeyWord(ref queryData, query.Search);

                var sortData = _sortHelper.ApplySort(queryData, query.OrderBy!);

                var data = await sortData.ToPagedListAsync(query.PageIndex, query.PageSize);

                var pagingData = new PagingModel<UserViewModel>()
                {
                    PageIndex = data.CurrentPage,
                    PageSize = data.PageSize,
                    TotalCount = data.TotalCount,
                    pagingData = _mapper.Map<List<User>, List<UserViewModel>>(data)
                };
                return pagingData;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<UserViewModel> GetById(Guid id)
        {
            try
            {
                var data = await GetUser(id);
                if (data == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }
                return _mapper.Map<User, UserViewModel>(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<Guid> Update(Guid id, UserUpdateModel model)
        {
            try
            {
                var checkExistUser = await GetUser(id);
                if (checkExistUser == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }
                var updateData = _mapper.Map(model, checkExistUser);
                _dataContext.Users.Update(updateData);
                await _dataContext.SaveChangesAsync();
                return checkExistUser.Id;
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
                var checkExistUser = await GetUser(id);
                if (checkExistUser == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }
                checkExistUser.IsDeleted = true;
                _dataContext.Users.Update(checkExistUser);
                await _dataContext.SaveChangesAsync();
                return checkExistUser.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<Guid> UpdateProfileImage(string id, IFormFile image)
        {
            try
            {
                var checkExistUser = await GetUser(new Guid(id));
                if (checkExistUser == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }
                string avatarPath = id.ToString() + "/Avatar";
                UserUpdateModel profile = new UserUpdateModel
                {
                    Avatar = await _firebaseStorageService.UploadFileAsync(image, avatarPath, Guid.NewGuid().ToString() + image.FileName),
                };
                var updateData = _mapper.Map(profile, checkExistUser);
                _dataContext.Users.Update(updateData);
                await _dataContext.SaveChangesAsync();
                return checkExistUser.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<Guid> UploadDocument(string id, IdentificationInformation model)
        {
            try
            {
                var checkExistUser = await GetUser(new Guid(id));
                if (checkExistUser == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }
                string avatarPath = id.ToString() + "/Identification Document";
                UserUpdateModel profile = new UserUpdateModel
                {
                    IdentityNumber = model.IdentityNumber,
                    IdentityCardProvideDate = EnsureDateUtc(model.IdentityCardProvideDate),
                    IdentityCardFrontImage = await _firebaseStorageService.UploadFileAsync(model.IdentityCardFrontImage, avatarPath, model.IdentityCardFrontImage.FileName + "_" + id),
                    IdentityCardBackImage = await _firebaseStorageService.UploadFileAsync(model.IdentityCardBackImage, avatarPath, model.IdentityCardBackImage.FileName + "_" + id),
                    Status = UserStatus.Inactive
                };
                var updateData = _mapper.Map(profile, checkExistUser);
                _dataContext.Users.Update(updateData);
                await _dataContext.SaveChangesAsync();
                return checkExistUser.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        public async Task<Guid> ApproveIdentificationDocument(Guid id, ApprovedIdentificationDocument model, string approvedById)
        {
            try
            {
                var checkExistUser = await GetUser(id);
                if (checkExistUser == null)
                {
                    throw new AppException(ErrorMessage.IdNotExist);
                }
                if (model.IsApproved)
                {
                    checkExistUser.Status = UserStatus.Active;
                }
                else
                {
                    checkExistUser.Status = UserStatus.Rejected;
                }
                _dataContext.Users.Update(checkExistUser);
                await _dataContext.SaveChangesAsync();

                return checkExistUser.Id;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new AppException(e.Message);
            }
        }

        // private method

        private void SearchByKeyWord(ref IQueryable<User> users, string? keyword)
        {
            if (!users.Any() || string.IsNullOrWhiteSpace(keyword))
                return;
            users = users.Where(o => o.Name.ToLower().Contains(keyword.Trim().ToLower()) || o.UserName.ToLower().Contains(keyword.Trim().ToLower()));
        }
        private async Task<User> GetUser(Guid id)
        {
            try
            {
                var data = await _dataContext
                    .Users
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
        private DateTime EnsureDateUtc(DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Unspecified)
            {
                // Assuming your original DateTime is in the local time zone
                return DateTime.SpecifyKind(dateTime, DateTimeKind.Local).ToUniversalTime();
            }
            else if (dateTime.Kind == DateTimeKind.Local)
            {
                return dateTime.ToUniversalTime();
            }
            else
            {
                return dateTime;
            }
        }
    }
}
