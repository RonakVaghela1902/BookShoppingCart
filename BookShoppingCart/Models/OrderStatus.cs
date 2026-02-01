using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShoppingCart.Models
{
    [Table("OrderStatuses")]
    public class OrderStatus
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int StatusId { get; set; }
        [Required,MaxLength(20)]
        public string? StatusName { get; set; }
    }
}
