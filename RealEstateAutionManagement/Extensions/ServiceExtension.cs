using Data.EFCore;
using Data.Entities;
using Data.Models;
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
            services.AddScoped<IAuctionService, AuctionService>();
            services.AddScoped<ISortHelpers<User>, SortHelper<User>>();
            services.AddScoped<ISortHelpers<Auction>, SortHelper<Auction>>();
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
    }
}
