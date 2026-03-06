using CatalogService.Repository;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace CatalogService.Controllers;

[ApiController]
[Route("[controller]")]
public class CatalogController : ControllerBase
{
    private readonly IProduct _repository;

    public CatalogController(IProduct repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IEnumerable<Product>> Get()
    {
        return await _repository.GetAll();
    }

    [HttpGet("{id}")]
    public async Task<Product> Get(Guid id)
    {
        return await _repository.GetById(id);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        await _repository.Create(product);
        return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task Update(Guid id, Product product)
    {
        await _repository.Update(id, product);
    }

    [HttpDelete("{id}")]
    public async Task Delete(Guid id)
    {
        await _repository.Delete(id);
    }
}