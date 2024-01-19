using Data.EFCore;
using Data.Entities;
using Data.Models;
<<<<<<< Updated upstream
using Hangfire;
using Hangfire.PostgreSql;
=======
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
>>>>>>> Stashed changes
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RealEstateAuctionManagement.Extensions;
using Service.Core;
using Service.Utilities;
using System.Text;

namespace UserManagement.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigurePostgreSqlServer(this IServiceCollection services, DbSetupModel model)
        {
            services.AddDbContext<DataContext>(o =>
            {
                o.UseNpgsql(model?.ConnectionStrings);
            });
        }

        public static void ConfigCors(this IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("AllowAll", builder =>
                    builder.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin())
            );
        }

        public static void AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IJwtUtils, JwtUtils>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISortHelpers<User>, SortHelper<User>>();

            services.AddScoped<IAuctionService, AuctionService>();
            services.AddScoped<ISortHelpers<Auction>, SortHelper<Auction>>();

            services.AddScoped<IBankAccountService, BankAccountService>();
            services.AddScoped<ISortHelpers<BankAccount>, SortHelper<BankAccount>>();

            services.AddScoped<ICategoriesService, CategoriesService>();
            services.AddScoped<ISortHelpers<Category>, SortHelper<Category>>();

            services.AddScoped<IRealEstateService, RealEstateService>();
            services.AddScoped<ISortHelpers<RealEstate>, SortHelper<RealEstate>>();

            services.AddScoped<IRealEstateImageService, RealEstateImageService>();
            services.AddScoped<ISortHelpers<RealEstateImage>, SortHelper<RealEstateImage>>();

            services.AddScoped<ISettingService, SettingService>();
            services.AddScoped<ISortHelpers<Setting>, SortHelper<Setting>>();

            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<ISortHelpers<Transaction>, SortHelper<Transaction>>();

            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<ISortHelpers<Feedback>, SortHelper<Feedback>>();

            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<ISortHelpers<Notification>, SortHelper<Notification>>();
        }

        public static void ConfigureJWTToken(this IServiceCollection services, JwtModel? model)
        {
            services
                .AddAuthentication(op =>
                {
                    op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    op.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = model?.ValidAudience,
                        ValidIssuer = model?.ValidIssuer,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(model?.Secret ?? ""))
                    };
                });
        }

<<<<<<< Updated upstream
        public static void ConfigureHangire(this IServiceCollection services, DbSetupModel model)
        {
            services.AddHangfire(config => config
                .UsePostgreSqlStorage(c => c.UseNpgsqlConnection(model?.ConnectionStrings))
                .UseFilter(new AutomaticRetryAttribute { Attempts = 0 }));
            services.AddHangfireServer(options =>
            {
                options.WorkerCount = Environment.ProcessorCount * 5;
                options.Queues = new[] { "critical", "default" };
            });
=======
        public static void ConfigureFirebaseServices(this IServiceCollection services, FirebaseModel model)
        {
            var credential = GoogleCredential.FromFile(Environment.CurrentDirectory! + "\\" + model.FirebaseSDKFile);

            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions
                {
                    Credential = credential,
                    ProjectId = model.ProjectId
                });
            }
            StorageClient _storageClient = StorageClient.Create(credential);
            services.AddSingleton<IFirebaseStorageService>(new FirebaseStorageService(model.Bucket, _storageClient));
>>>>>>> Stashed changes
        }
    }
}
