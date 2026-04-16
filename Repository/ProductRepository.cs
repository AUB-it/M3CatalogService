using CatalogService.Repository;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using Models;

public class ProductRepository : IProduct
{
    private readonly IMongoCollection<Product> _products;

    public ProductRepository(IConfiguration configuration)
    {
        // Hent MongoDB connection string fra miljøvariabel
        var connectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING");
        var databaseName = "CatalogDb"; // kan også sættes via miljøvariabel
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        _products = database.GetCollection<Product>("Products");
    }

    public async Task<List<Product>> GetAll() =>
        await _products.Find(_ => true).ToListAsync();

    public async Task<Product> GetById(int id) =>
        await _products.Find(p => p.Id == id).FirstOrDefaultAsync();

    public async Task Create(Product product) =>
        await _products.InsertOneAsync(product);

    public async Task Update(int id, Product product) =>
        await _products.ReplaceOneAsync(p => p.Id == id, product);

    public async Task Delete(int id) =>
        await _products.DeleteOneAsync(p => p.Id == id);
}