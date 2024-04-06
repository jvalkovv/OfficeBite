using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OfficeBite.Infrastructure.Data.Models
{
    public class OrderHistory
    {
        [Comment("Order history identifier")]
        [Key]
        public int Id { get; set; }

        [Comment("Order id")]
        [Required]
        public int OrderId { get; set; }

        [Comment("Connection with Id from Order table")]
        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; } = null!;
    }
}