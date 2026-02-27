using Microsoft.AspNetCore.Mvc;
using Models;

namespace CatalogService.Controllers;

[ApiController]
[Route("[controller]")]
public class CatalogController : ControllerBase
{
    private readonly ILogger<CatalogController> _logger;

    // Her skal _products være inde i klassen
    private static List<Product> _products = new List<Product>()
    {
        new()
        {
            Id = new Guid("7125e019-c469-4dbd-93e5-426de6652523"),
            Name = "Salmon Fillet",
            Description = "Fresh salmon fillet",
            Price = 12.99m,
            Brand = "FishmongerX",
            Manufacturer = "Fish Supplier",
            Model = "Standard",
            ImageUrl = "https://example.com/salmon.jpg",
            ProductUrl = "https://example.com/salmon",
            ReleaseDate = DateTime.Now,
            ExpiryDate = DateTime.Now.AddDays(3),
        }
    };

    public CatalogController(ILogger<CatalogController> logger)
    {
        _logger = logger;
    }

    [HttpGet("{productId}", Name = "GetProductById")]
    public Product Get(Guid productId)
    {
        return _products.Where(p => p.Id == productId).First();
    }
}