using FCOrderManagement.Models;
using FCOrderManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace FCOrderManagement.Controllers
{
    [ApiController]
    [Route("api/Orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IApiService _apiService;
        private readonly OrderCalculationService _calculationService;

        public OrdersController(IApiService apiService, OrderCalculationService calculationService)
        {
            _apiService = apiService;
            _calculationService = calculationService;
        }

        [HttpPost("place")]
        public async Task<IActionResult> PlaceOrder([FromBody] Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var products = await _apiService.GetProducts();

            _calculationService.CalculateOrderTotals(order, products);

            var success = await _apiService.PlaceOrder(order);
            if (success)
            {
                return Ok(new { message = "Order placed successfully", order = order });
            }
            return BadRequest(new { message = "Failed to place order" });
        }
    }
}