using FCOrderManagement.Models;
using Microsoft.Extensions.Configuration;

public class OrderCalculationService
{
    private readonly IConfiguration _configuration;

    public OrderCalculationService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void CalculateOrderTotals(Order order, List<Product> products)
    {
        // Calculate total sales value excluding VAT
        order.SalesValueExcludingVAT = order.OrderDetails.Sum(od =>
            od.Quantity * products.First(p => p.ProductID == od.ProductID).Price);

        // Apply discount
        order.Discount = CalculateDiscount(order.SalesValueExcludingVAT);

        // Calculate total including VAT
        decimal vatRate = _configuration.GetValue<decimal>("OrderSettings:VATRate");
        order.SalesValueIncludingVAT = (order.SalesValueExcludingVAT - order.Discount) * (1 + vatRate);
    }

    private decimal CalculateDiscount(decimal totalBeforeDiscount)
    {
        var discountTiers = _configuration.GetSection("OrderSettings:DiscountTiers")
            .Get<List<DiscountTier>>()
            .OrderByDescending(t => t.Threshold);

        foreach (var tier in discountTiers)
        {
            if (totalBeforeDiscount >= tier.Threshold)
            {
                return totalBeforeDiscount * tier.DiscountRate;
            }
        }

        return 0;
    }
}

public class DiscountTier
{
    public decimal Threshold { get; set; }
    public decimal DiscountRate { get; set; }
}