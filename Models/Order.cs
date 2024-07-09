// Models/Order.cs
using System.ComponentModel.DataAnnotations;

public class Order
{
    public int OrderID { get; set; }

    [Required]
    public string UserID { get; set; }

    [Required]
    public DateTime OrderDate { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string CustomerName { get; set; }

    public decimal SalesValueExcludingVAT { get; set; }
    public decimal Discount { get; set; }
    public decimal SalesValueIncludingVAT { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "An order must have at least one product")]
    public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}