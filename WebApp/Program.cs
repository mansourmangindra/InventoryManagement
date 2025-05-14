using Common.Constants;
using Common.DataTransferObjects._Core.AppSettings;
using WebApp.Services.Interfaces;
using WebApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using WebApp.Authorization.Handlers;
using WebApp.Authorization.Requirements;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

HttpResourceEndpoint httpResourceEndpoint = new();
builder.Configuration.Bind(nameof(HttpResourceEndpoint), httpResourceEndpoint);
builder.Services.AddSingleton(httpResourceEndpoint);

CacheSetting cacheSetting = new();
builder.Configuration.Bind(nameof(CacheSetting), cacheSetting);
builder.Services.AddSingleton(cacheSetting);




//IServices
builder.Services.AddSingleton<IReferenceDataService, ReferenceDataService>();
builder.Services.AddSingleton<ISecurityService, SecurityService>();
builder.Services.AddSingleton<IAuthorizationHandler, ModuleHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, RoleHandler>();
builder.Services.AddSingleton<ITokenProvider, TokenProvider>();


builder.Services.AddHttpContextAccessor();


builder.Services.AddHttpClient(HttpClientConstant.InventoryApiNamedClient, opt =>
{
    opt.Timeout = TimeSpan.FromMinutes(5);
    opt.BaseAddress = new Uri(httpResourceEndpoint.InventoryApiBaseUrl);
});


/*RESPONSE HEADER & SECURITY*/
builder.Services.AddHsts(options =>
{
    options.MaxAge = TimeSpan.FromDays(365);
});

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "Inventory-RV-Token";
});

//Security Group Policy
SecurityGroup securitygroup = new();
builder.Configuration.Bind("SecurityGroup", securitygroup);
builder.Services.AddAuthorization(options => 
{
    //ErrorLog Policy with Role
    options.AddPolicy(InventoryPolicyConstant.ErrorLogPolicy, policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.Requirements.Add(new RoleRequirement(new short[] { IMConstant.ADMIN }));
    });

    options.AddPolicy(InventoryPolicyConstant.ErrorLogPolicy, policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.Requirements.Add(new RoleRequirement(new short[] { IMConstant.ADMIN }));
    });


});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromHours(10);
        options.LoginPath = "/Login/Login";
        options.AccessDeniedPath = "/Error/AccessDenied";

        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = ctx =>
            {
                ctx.Response.Redirect($"/Error/StatusPage?code=401");
                return Task.CompletedTask;
            },
            OnRedirectToAccessDenied = ctx =>
            {
                ctx.Response.Redirect($"/Error/StatusPage?code=403");
                return Task.CompletedTask;
            }
        };
    });


builder.Services.AddControllersWithViews(opt =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        //TODO: Remove below line if all users in tenent are allowed to access the application
        .Build();
    //opt.Filters.Add(new AuthorizeFilter(policy));

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
