using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace CatalogService.Pages
{
    public class CreateProduct : PageModel
    {
        private readonly IHttpClientFactory? _clientFactory = null;
        
        [BindProperty]
        public ProductDTO? Product { get; set; }
        
        public CreateProduct(IHttpClientFactory clientFactory)
            => _clientFactory = clientFactory;

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            using HttpClient? client = _clientFactory?.CreateClient("HaavGateway");
            try
            {
                var product = new ProductDTO
                {
                    Name = Product?.Name,
                    Brand = Product?.Brand,
                    Model = Product?.Model,
                    Description = Product?.Description,
                    Sku = Product?.Sku,
                    Price = Product?.Price ?? 0,
                    Currency = Product?.Currency,
                    ReleaseDate = DateTime.UtcNow
                };

                await client?.PostAsJsonAsync("catalog", product);
                return RedirectToPage("/CatalogList/CatalogList");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Page();
            }
        }
    }
}