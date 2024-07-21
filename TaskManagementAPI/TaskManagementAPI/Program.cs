using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using TaskManagementAPI.Common;

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
var virtualDirectory = builder.Configuration.GetSection("AppSettings").GetValue<string>("VirtualDirectory");
builder.Logging.AddSerilog(logger);
builder.Services.AddCors();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
});
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
