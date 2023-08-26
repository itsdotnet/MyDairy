using Microsoft.EntityFrameworkCore;
using MyDairy.DAL.Constexts;
using MyDairy.Service.Helpers;
using MyDairy.WebApi.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//add db
builder.Services.AddDbContext<MyDairyDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//add jwt
builder.Services.AddJwt(builder.Configuration);

//add log
var logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext()
        .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

//add services
builder.Services.AddServices();

//get root path
PathHelper.WebRootPath = Path.GetFullPath("wwwroot");

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
name: "default",
pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();