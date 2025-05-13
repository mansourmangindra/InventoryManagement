using Common.Constants;
using Common.DataTransferObjects._Core.AppSettings;
using WebApp.Services.Interfaces;
using WebApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

HttpResourceEndpoint httpResourceEndpoint = new();
builder.Configuration.Bind(nameof(HttpResourceEndpoint), httpResourceEndpoint);
builder.Services.AddSingleton(httpResourceEndpoint);

CacheSetting cacheSetting = new();
builder.Configuration.Bind(nameof(CacheSetting), cacheSetting);
builder.Services.AddSingleton(cacheSetting);

builder.Services.AddControllersWithViews();


//IServices
builder.Services.AddSingleton<IReferenceDataService, ReferenceDataService>();
builder.Services.AddSingleton<ISecurityService, SecurityService>();
builder.Services.AddSingleton<ITokenProvider, TokenProvider>();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddHttpContextAccessor();


builder.Services.AddHttpClient(HttpClientConstant.InventoryApiNamedClient, opt =>
{
    opt.Timeout = TimeSpan.FromMinutes(5);
    opt.BaseAddress = new Uri(httpResourceEndpoint.InventoryApiBaseUrl);
});

//IServices
builder.Services.AddSingleton<IReferenceDataService, ReferenceDataService>();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddHttpContextAccessor();



/*RESPONSE HEADER & SECURITY*/
builder.Services.AddHsts(options =>
{
    options.MaxAge = TimeSpan.FromDays(365);
});

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "Inventory-RV-Token";
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromHours(10);
        options.LoginPath = "/Login/Login";
        options.AccessDeniedPath = "/Login/AccessDenied";
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithRedirects("/Error/StatusPage?code={0}");
app.UseExceptionHandler("/Error/LogError");
app.UseForwardedHeaders();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

await app.RunAsync();
