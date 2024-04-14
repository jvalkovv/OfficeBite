using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OfficeBite.Infrastructure.Data.Models
{
    public class OrderHistory
    {
        [Comment("Order History identifier")]
        [Key]
        public int Id { get; set; }

        [Comment("User identifier")]
        public string UserAgentId { get; set; } = string.Empty;
        public int MenuOrderRequestNumber { get; set; }

        [Comment("Order id")]
        [Required]
        public int OrderId { get; set; }

        [Comment("Connection with Id from Order table")]
        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; } = null!;


    }
}