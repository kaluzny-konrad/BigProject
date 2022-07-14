using System.ComponentModel.DataAnnotations;

namespace BigProject.Models;

public class OrderModel
{
    public int OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    [Required]
    [MinLength(4)]
    public string OrderNumber { get; set; }
    public ICollection<OrderItemModel> Items { get; set; }
}
