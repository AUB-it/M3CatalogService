using Models;

namespace CatalogService.Repository;

public interface IProduct
{
        Task<List<Product>> GetAll();
        Task<Product> GetById(Guid id);
        Task Create(Product product);
        Task Update(Guid id, Product product);
        Task Delete(Guid id);
}