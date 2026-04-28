using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
namespace CatalogService.Pages
{
    public class CatalogList : PageModel
    {
        private readonly IHttpClientFactory? _clientFactory = null;
        public List<ProductDTO>? Products {get; set;}
        public CatalogList(IHttpClientFactory clientFactory)
            => _clientFactory = clientFactory;
        public void OnGet()
        {
            using HttpClient? client = _clientFactory?.CreateClient("HaavGateway");
            try
            {
                Products = client?.GetFromJsonAsync<List<ProductDTO>>(
                    "catalog").Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
    
    public class ProductDTO
    {
        public Guid ProductId { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }

        public string? Sku { get; set; }
        public string? Brand { get; set; }
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }

        public decimal Price { get; set; }
        public string? Currency { get; set; }

        public string? ImageUrl { get; set; }
        public string? ProductUrl { get; set; }

        public DateTime ReleaseDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}