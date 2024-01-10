using Data.EFCore;
using Data.Models;
using Microsoft.EntityFrameworkCore;

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

        }

        //public static void ConfigureJWTToken(this IServiceCollection services, JwtModel? model)
        //{
        //    services
        //        .AddAuthentication(op =>
        //        {
        //            op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //            op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //            op.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        //        })
        //        .AddJwtBearer(options =>
        //        {
        //            options.SaveToken = true;
        //            options.RequireHttpsMetadata = false;
        //            options.TokenValidationParameters = new TokenValidationParameters()
        //            {
        //                ValidateIssuer = true,
        //                ValidateAudience = true,
        //                ValidAudience = model?.ValidAudience,
        //                ValidIssuer = model?.ValidIssuer,
        //                IssuerSigningKey =
        //                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(model?.Secret ?? ""))
        //            };
        //        });
        //}
    }
}
