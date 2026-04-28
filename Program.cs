using CatalogService.Repository;
using DotNetEnv;
using Scalar.AspNetCore;
using NLog;
using NLog.Web;
using Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

var logger = NLog.LogManager.Setup()
    .LoadConfigurationFromAppSettings()
    .GetCurrentClassLogger();

try 
{
    logger.Debug("start min service"); 

    var builder = WebApplication.CreateBuilder(args);

    Env.Load();

    builder.Services.AddMemoryCache();
    builder.Services.AddRazorPages();
    builder.Services.AddControllers();
    builder.Services.AddOpenApi();
    builder.Services.AddSingleton<IProduct, ProductRepository>();
    
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    
    var gatewayUrl = builder.Configuration["GatewayUrl"] ?? "http://localhost:4000/";
    builder.Services.AddHttpClient("HaavGateway", client =>
    {
        client.BaseAddress = new Uri(gatewayUrl);
        client.DefaultRequestHeaders.Add(
            Microsoft.Net.Http.Headers.HeaderNames.Accept, "application/json");
    });
    
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
    
    app.UseStaticFiles();
    app.MapGet("/", () => Results.Redirect("/Catalog"));
    app.MapRazorPages();

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