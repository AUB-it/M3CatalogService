using Models;

namespace CatalogService.Repository;

public interface IProduct
{
        Task<List<Product>> GetAll();
        Task<Product> GetById(int id);
        Task Create(Product product);
        Task Update(int id, Product product);
        Task Delete(int id);
}