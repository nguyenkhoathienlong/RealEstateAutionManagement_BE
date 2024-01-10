using Data.Models;
using Service.Mapper;
using UserManagement.Extensions;
using UserManagement.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigurePostgreSqlServer(builder.Configuration.GetSection("DbSetup").Get<DbSetupModel>());
builder.Services.AddAutoMapper(typeof(MapperProfiles));
builder.Services.AddCors();
// builder.Services.ConfigureJWTToken(builder.Configuration.GetSection("JWT").Get<JwtModel>());
builder.Services.AddBusinessServices();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapControllers();

app.Run();
