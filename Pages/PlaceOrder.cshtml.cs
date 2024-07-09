using FCOrderManagement.Models;
using FCOrderManagement.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

public class PlaceOrderModel : PageModel
{
    private readonly IApiService _apiService;

    public PlaceOrderModel(IApiService apiService)
    {
        _apiService = apiService;
    }

    [BindProperty]
    public Order Order { get; set; }

    public List<Product> Products { get; set; }

    public async Task OnGetAsync()
    {
        Products = await _apiService.GetProducts();
    }

    public async Task<IActionResult> OnPostAsync(int[] Quantities)
    {
        if (!ModelState.IsValid)
        {
            Products = await _apiService.GetProducts();
            return Page();
        }

        Order.OrderDetails = new List<OrderDetail>();
        for (int i = 0; i < Products.Count; i++)
        {
            if (Quantities[i] > 0)
            {
                Order.OrderDetails.Add(new OrderDetail
                {
                    ProductID = Products[i].ProductID,
                    Quantity = Quantities[i]
                });
            }
        }

        if (Order.OrderDetails.Count == 0)
        {
            ModelState.AddModelError(string.Empty, "At least one product must be selected");
            Products = await _apiService.GetProducts();
            return Page();
        }

        var success = await _apiService.PlaceOrder(Order);
        if (success)
        {
            return RedirectToPage("OrderConfirmation");
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Failed to place order");
            Products = await _apiService.GetProducts();
            return Page();
        }
    }
}