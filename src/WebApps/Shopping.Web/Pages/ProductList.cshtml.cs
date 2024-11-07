using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Shopping.Web.Models.Basket;
using Shopping.Web.Models.Catalog;
using Shopping.Web.Services;

namespace Shopping.Web.Pages
{
    public class ProductListModel : PageModel
    {
        private readonly ILogger<ProductDetailModel> logger;
        private readonly ICatalogService catalogService;
        private readonly IBasketService basketService;

        public ProductListModel(
            ILogger<ProductDetailModel> logger,
            ICatalogService catalogService,
            IBasketService basketService)
        {
            this.logger = logger;
            this.catalogService = catalogService;
            this.basketService = basketService;
        }

        public IEnumerable<string> CategoryList { get; set; } = [];
        public IEnumerable<ProductModel> ProductList { get; set; } = [];

        [BindProperty(SupportsGet = true)]
        public string SelectedCategory { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string categoryName)
        {
            var response = await catalogService.GetProducts();

            CategoryList = response.Products.SelectMany(p => p.Category).Distinct();

            if (!string.IsNullOrWhiteSpace(categoryName))
            {
                ProductList = response.Products.Where(p => p.Category.Contains(categoryName));
                SelectedCategory = categoryName;
            }
            else
            {
                ProductList = response.Products;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(Guid productId)
        {
            logger.LogInformation("Add to cart button clicked");
            var productResponse = await catalogService.GetProduct(productId);

            var basket = await basketService.LoadUserBasket();

            basket.Items.Add(new ShoppingCartItemModel
            {
                ProductId = productId,
                ProductName = productResponse.Product.Name,
                Price = productResponse.Product.Price,
                Quantity = 1,
                Color = "Black"
            });

            await basketService.StoreBasket(new StoreBasketRequest(basket));

            return RedirectToPage("Cart");
        }
    }
}