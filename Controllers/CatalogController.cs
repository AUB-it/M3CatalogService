using System.Diagnostics;
using CatalogService.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Models;

namespace CatalogService.Controllers;

[ApiController]
[Route("[controller]")]
public class CatalogController : ControllerBase
{
    private readonly IProduct _repository;
    private readonly ILogger<CatalogController> _logger;
    
    public CatalogController(IProduct repository, ILogger<CatalogController> logger)
    {
        _repository = repository;
        _logger = logger;

        var hostName = System.Net.Dns.GetHostName();
        var ips = System.Net.Dns.GetHostAddresses(hostName);
        var _ipaddr = ips.First().MapToIPv4().ToString();
        _logger.LogInformation(1, $"XYZ Service responding from {_ipaddr}");
    }

    [HttpGet]
    public async Task<IEnumerable<Product>> Get()
    {
        _logger.LogDebug("Henter alle produkter");
        return await _repository.GetAll();
    }
    
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        _logger.LogDebug("Henter product med id: {id}", id);
        
        var product = _repository.GetProductFromCache(id);
        if (product == null)
        {
            _logger.LogInformation("GetById hentet fra database");
            product = await _repository.GetById(id);
            
            // Store new value from DB in cache
            if (product != null)
            {
                _logger.LogInformation("Product gemt i cachen");
                _repository.SetProductInCache(product);
            }
        }
        else
        {
            _logger.LogInformation("Product hentet fra cache");
        }
        return Ok(product);
    }

    [HttpGet("version")]
    public async Task<Dictionary<string, string>> GetVersion()
    {
        var properties = new Dictionary<string, string>();
        var ver = FileVersionInfo.GetVersionInfo(typeof(Program).Assembly.Location).ProductVersion;
        
        // Logger versionen så det kan ses i log-filen (Opgave D)
        _logger.LogInformation("Service version forespurgt: {version}", ver);

        properties.Add("service", "HaaV Catalog Service");
        properties.Add("version", ver!);

        try
        {
            var hostName = System.Net.Dns.GetHostName();
            var ips = await System.Net.Dns.GetHostAddressesAsync(hostName);
            var ipa = ips.First().MapToIPv4().ToString();
            properties.Add("hosted-at-address", ipa);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Kunne ikke finde host-adresse");
            properties.Add("hosted-at-address", "Could not resolve IP-address");
        }

        return properties;
    }
}