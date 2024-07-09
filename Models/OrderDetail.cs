using System.ComponentModel.DataAnnotations;

public class OrderDetail
{
    public int OrderDetailID { get; set; }
    public int OrderID { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int ProductID { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}