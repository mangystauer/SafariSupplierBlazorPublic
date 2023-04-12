using DataAccessLibrary;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SuppliersBlazor.Data;


using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using SuppliersBlazor.Areas.Identity;
using DataAccessLibrary.Models;
using DataAccessLibrary.DataService.Shatem;
using DataAccessLibrary.Models.Shatem.DataAccess;
using DataAccessLibrary.ApiDataAccess;
using System.Configuration;
using DataAccessLibrary.Helpers;

var ConfBuilder = new ConfigurationBuilder()
.SetBasePath(Directory.GetCurrentDirectory()) //<--You would need to set the path
.AddJsonFile("appsettings.json"); //or what ever file you have the settings

IConfiguration configuration = ConfBuilder.Build();


var builder = WebApplication.CreateBuilder(args);



string connectionStringAuthDb = configuration.GetConnectionString("AuthDb");

//Auth added manually
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySQL(connectionStringAuthDb));
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


// Add services to the container.
//builder.Services.AddSingleton<IConfiguration>(configuration);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();

builder.Services.AddTransient<IDataAccess, MySQLDataAccess>();
builder.Services.AddTransient<ICrud, MySqlCrud>();
builder.Services.AddHttpClient();
builder.Services.AddSingleton(configuration.GetSection("AutoTradeAccess").Get<AutoTradeAccess>());


//ShatemApi
builder.Services.Configure<ShatemConfig>(configuration.GetSection("ShatemConfig"));
builder.Services.AddScoped<IShatemDataService, ShatemDataService>();
builder.Services.AddTransient<IShatemAccess, ShatemAccess>();


//Redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = configuration.GetConnectionString("Redis");
    options.InstanceName = "Safari";

});
builder.Services.AddSingleton<RedisHelper>(sp => new RedisHelper(configuration.GetConnectionString("Redis")));



var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

//app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();


