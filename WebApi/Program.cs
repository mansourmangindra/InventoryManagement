using ApiConfiguration;
using Common.DataTransferObjects._Core.AppSettings;
using Common.DataTransferObjects._Core.ErrorLog;
using DataAccess.DbContexts.InventoryManagement;
using DataAccess.Services;
using DataAccess.Services.Interfaces;
using DataAccess.UnitOfWorks.InventoryManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using WebApi.Services.Registration;
using WebAPI.Services.Error;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

IdentityServerApiDefinition identityServerApiDefinition = new();
builder.Configuration.Bind(nameof(IdentityServerApiDefinition), identityServerApiDefinition);
builder.Services.AddSingleton(identityServerApiDefinition);

//DBContext Registration
builder.Services.AddDbContext<InventoryManagementDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("INVENTORYDB"));
});

//UnitOfWork Registration
builder.Services.AddScoped<IInventoryUnitOfWork, InventoryUnitOfWork>();
builder.Services.AddScoped<IDbContextChangeTrackingService, DbContextChangeTrackingService > ();
builder.Services.AddHttpContextAccessor();

//Services Registration

builder.Services.AddScoped<IErrorLogService, ErrorLogService>();
builder.Services.AddScoped<IRegistration, Registration>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



ApiServices.ConfigureServices(builder.Services, identityServerApiDefinition);



var app = builder.Build();

app.UseExceptionHandler(errorLogger =>
{
    errorLogger.Run(async context =>
    {
        var scoped = app.Services.CreateScope();
        IErrorLogService errorLogService = scoped.ServiceProvider.GetRequiredService<IErrorLogService>();
        context.Response.StatusCode = 500;
        ErrorMessage unhandledErrorDetail = await errorLogService.LogApiError(context);
        await context.Response.WriteAsync(JsonConvert.SerializeObject(unhandledErrorDetail));
    });
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
