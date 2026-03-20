using CatalogService.Repository;
using DotNetEnv;
using Scalar.AspNetCore;
using NLog;
using NLog.Web;
using Models;
using Microsoft.AspNetCore.Mvc;

var logger = NLog.LogManager.Setup()
    .LoadConfigurationFromAppSettings()
    .GetCurrentClassLogger();

try 
{
    logger.Debug("start min service"); 

    var builder = WebApplication.CreateBuilder(args);

    Env.Load();
    
    builder.Services.AddControllers();
    builder.Services.AddOpenApi();
    builder.Services.AddSingleton<IProduct, ProductRepository>();
    
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var app = builder.Build();
    
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference();
    }

    app.UseHttpsRedirection();

    app.UseCors(policy => policy
        .SetIsOriginAllowed(origin =>
        {
            if (string.IsNullOrEmpty(origin)) return false;
            try { return new Uri(origin).Host == "localhost"; }
            catch { return false; }
        })
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
    );

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    
    NLog.LogManager.Shutdown();
}