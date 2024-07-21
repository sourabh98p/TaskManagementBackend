using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Reflection;
using TaskManagementAPI.Common;
using TaskManagementAPI.Manager.Abstract;
using TaskManagementAPI.Manager.Concrete;
using TaskManagementAPI.Repository.Abstract;
using TaskManagementAPI.Repository.Concrete;

var builder = WebApplication.CreateBuilder(args);
var environment = builder.Environment;
builder.Configuration
    .SetBasePath(environment.ContentRootPath)
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("connectionStrings"));
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
builder.Services.AddCors();
builder.Services.AddControllers();
var timeZone = DateTime.Now;
var timeZOne2 = DateTime.UtcNow;
var DBConnectionString = builder.Configuration.GetSection("ConnectionStrings").GetValue<string>("DBConnectionString");
//builder.Services.AddDbContext<AppDbContext>(options =>
//        options.UseNpgsql(DBConnectionString)
//    );

builder.Services.AddTransient<IAuthenticateManager, AuthenticateManager>();
builder.Services.AddTransient<IAuthenticateRepository, AuthenticateRepository>();
builder.Services.AddTransient<ITaskManagementManager, TaskManagementManager>();
builder.Services.AddTransient<ITaskManagementRepository, TaskManagementRepository>();
var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
        .UseNpgsql(DBConnectionString) 
        .Options;
var dbContext = new AppDbContext(dbContextOptions);
builder.Services.AddSingleton<AppDbContext>(dbContext);
builder.Services.AddHttpContextAccessor();
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseRouting();
var localizeOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
if (localizeOptions != null)
{
    app.UseRequestLocalization(localizeOptions.Value);
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
