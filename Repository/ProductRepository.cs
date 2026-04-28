using CatalogService.Repository;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using Models;

public class ProductRepository : IProduct
{
    private readonly IMongoCollection<Product> _products;
    private readonly IMemoryCache _memoryCache;

    public ProductRepository(IConfiguration configuration, IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
        // Hent MongoDB connection string fra miljøvariabel
        var connectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING");
        var databaseName = "CatalogDb"; // kan også sættes via miljøvariabel
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        _products = database.GetCollection<Product>("Products");
    }

    public async Task<List<Product>> GetAll() =>
        await _products.Find(_ => true).ToListAsync();

    public async Task<Product> GetById(Guid id) =>
        await _products.Find(p => p.ProductId == id).FirstOrDefaultAsync();

    public async Task Create(Product product) =>
        await _products.InsertOneAsync(product);

    public async Task Update(Guid id, Product product) =>
        await _products.ReplaceOneAsync(p => p.ProductId == id, product);

    public async Task Delete(Guid id) =>
        await _products.DeleteOneAsync(p => p.ProductId == id);
    
    public void SetProductInCache(Product product)
    {
        var cacheExpiryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpiration = DateTime.Now.AddHours(1),
            SlidingExpiration = TimeSpan.FromMinutes(10),
            Priority = CacheItemPriority.High
        };
        _memoryCache.Set(product.ProductId, product, cacheExpiryOptions);
    }
    
    public Product? GetProductFromCache(Guid productId)
    {
        Product product = null;
        _memoryCache.TryGetValue(productId, out product);
        return product;
    }
    
    public void RemoveFromCache(Guid productId)
    {
        _memoryCache.Remove(productId);
    }
}