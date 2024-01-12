using AutoMapper;
using Data.Entities;
using Data.Models;

namespace Service.Mapper
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles() 
        {
            // User
            CreateMap<UserCreateModel, User>();
            CreateMap<User, UserViewModel>();
            CreateMap<UserUpdateModel, User>()
                .ForAllMembers(opt => opt.Condition((src, des, obj) => obj != null));

            // UserBid
            CreateMap<UserBidCreateModel, UserBid>();
            CreateMap<UserBid, UserBidViewModel>();
            CreateMap<UserBidUpdateModel, UserBid>()
                .ForAllMembers(opt => opt.Condition((src, des, obj) => obj != null));

            // Auction
            CreateMap<AuctionCreateModel, Auction>();
            CreateMap<Auction, AuctionViewModel>();
            CreateMap<AuctionUpdateModel, Auction>()
                .ForAllMembers(opt => opt.Condition((src, des, obj) => obj != null));

            // BankAccount
            CreateMap<BankAccountCreateModel, BankAccount>();
            CreateMap<BankAccount, BankAccountViewModel>();
            CreateMap<BankAccountUpdateModel, BankAccount>()
                .ForAllMembers(opt => opt.Condition((src, des, obj) => obj != null));

            // Category
            CreateMap<CategoryModel, Category>();
            CreateMap<Category, CategoryViewModel>();
            CreateMap<CategoryUpdateModel, Category>()
                .ForAllMembers(opt => opt.Condition((src, des, obj) => obj != null));

            // Feedback
            CreateMap<FeedbackCreateModel, Feedback>();
            CreateMap<Feedback, FeedbackViewModel>();
            CreateMap<FeedbackUpdateModel, Feedback>()
                .ForAllMembers(opt => opt.Condition((src, des, obj) => obj != null));

            // Notification
            CreateMap<NotificationCreateModel, Notification>();
            CreateMap<Notification, NotificationViewModel>();
            CreateMap<NotificationUpdateModel, Notification>()
                .ForAllMembers(opt => opt.Condition((src, des, obj) => obj != null));

            // RealEstate
            CreateMap<RealEstateCreateModel, RealEstate>();
            CreateMap<RealEstate, RealEstateViewModel>();
            CreateMap<RealEstateUpdateModel, RealEstate>()
                .ForAllMembers(opt => opt.Condition((src, des, obj) => obj != null));

            // RealEstateImage
            CreateMap<RealEstateImageCreateModel, RealEstateImage>();
            CreateMap<RealEstateImage, RealEstateImageViewModel>();
            CreateMap<RealEstateImageUpdateModel, RealEstateImage>()
                .ForAllMembers(opt => opt.Condition((src, des, obj) => obj != null));

            // Setting
            CreateMap<SettingCreateModel, Setting>();
            CreateMap<Setting, SettingViewModel>();
            CreateMap<SettingUpdateModel, Setting>()
                .ForAllMembers(opt => opt.Condition((src, des, obj) => obj != null));

            // Transaction
            CreateMap<TransactionCreateModel, Transaction>();
            CreateMap<Transaction, TransactionViewModel>();
            CreateMap<TransactionUpdateModel, Transaction>()
                .ForAllMembers(opt => opt.Condition((src, des, obj) => obj != null));
        }
    }
}
