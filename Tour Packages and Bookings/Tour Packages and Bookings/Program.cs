using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Tour_Packages_and_Bookings.HostedServices;
using Tour_Packages_and_Bookings.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TourDbContext>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("db")));
#region Hosted services
builder.Services.AddScoped<ApplyMigrationService>();
builder.Services.AddHostedService<MigrationHostedService>();
#endregion
builder.Services.AddControllersWithViews();
#region Identity Configuration
//builder.Services.AddIdentity<AppUser, IdentityRole>(op =>
//{
//    op.SignIn.RequireConfirmedAccount = false;
//    op.Password.RequiredLength = 6;
//    op.Password.RequireNonAlphanumeric = false;
//    op.Password.RequireDigit = false;
//    op.Password.RequireUppercase = false;
//    op.Password.RequireLowercase = false;
//}).AddEntityFrameworkStores<TourDbContext>();
//builder.Services.ConfigureApplicationCookie(op =>
//{
//    op.LoginPath = "/Account/Login";
//    op.Cookie.HttpOnly = true;
//});
#endregion

var app = builder.Build();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultControllerRoute();
app.Run();