using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Shopping.Web.Models.Ordering;
using Shopping.Web.Services;

namespace Shopping.Web.Pages
{
    public class OrderListModel : PageModel
    {
        private readonly ILogger<OrderListModel> logger;
        private readonly IOrderingService orderingService;

        public OrderListModel(ILogger<OrderListModel> logger, IOrderingService orderingService)
        {
            this.logger = logger;
            this.orderingService = orderingService;
        }

        public IEnumerable<OrderModel> Orders { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            logger.LogInformation("Order List page visited");

            // assumption customerId is passed in from the UI authenticated user swn
            var customerId = new Guid("58c49479-ec65-4de2-86e7-033c546291aa");

            var response = await orderingService.GetOrdersByCustomer(customerId);
            Orders = response.Orders;

            return Page();
        }
    }
}