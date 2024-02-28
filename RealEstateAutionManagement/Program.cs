using Data.Models;
using Hangfire;
using Microsoft.OpenApi.Models;
using Service.Mapper;
using Service.SignalR;
using RealEstateAuctionManagement.Extensions;
using RealEstateAuctionManagement.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigurePostgreSqlServer(builder.Configuration.GetSection("DbSetup").Get<DbSetupModel>()!);
builder.Services.AddAutoMapper(typeof(MapperProfiles));
builder.Services.ConfigCors();
builder.Services.ConfigureJWTToken(builder.Configuration.GetSection("JWT").Get<JwtModel>());
builder.Services.ConfigureFirebaseServices(builder.Configuration.GetSection("Firebase").Get<FirebaseModel>());
builder.Services.AddBusinessServices();
builder.Services.ConfigureHangire(builder.Configuration.GetSection("DbSetup").Get<DbSetupModel>()!);
builder.Services.ConfigureSignalR();
builder.Services.AddHttpContextAccessor();

// Logging
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
});

builder.Services.AddControllers(op =>
{
    op.Filters.Add(new ResultManipulator());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "REAS API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{ c.SwaggerEndpoint("/swagger/v1/swagger.json", "REAS API v1"); });

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseHangfireDashboard("/hangfire");

app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapControllers();

app.MapHub<SignalRHub>("/reasHub");

app.Run();
