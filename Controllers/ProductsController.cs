using FCOrderManagement.Models;
using FCOrderManagement.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IApiService _apiService;

    public OrdersController(IApiService apiService)
    {
        _apiService = apiService;
    }

    [HttpPost]
    public async Task<IActionResult> PlaceOrder([FromBody] Order order)
    {
        // Calculate total sales value
        order.SalesValueExcludingVAT = order.OrderDetails.Sum(od => od.Quantity * _apiService.GetProducts().Result.First(p => p.ProductID == od.ProductID).Price);

        // Calculate discount
        if (order.SalesValueExcludingVAT > 500)
        {
            order.Discount = order.SalesValueExcludingVAT * 0.1m;
        }
        else if (order.SalesValueExcludingVAT > 200)
        {
            order.Discount = order.SalesValueExcludingVAT * 0.03m;
        }

        // Calculate total including VAT (assuming VAT is 15%)
        order.SalesValueIncludingVAT = (order.SalesValueExcludingVAT - order.Discount) * 1.15m;

        var success = await _apiService.PlaceOrder(order);
        if (success)
        {
            return Ok(new { message = "Order placed successfully" });
        }
        return BadRequest(new { message = "Failed to place order" });
    }
}