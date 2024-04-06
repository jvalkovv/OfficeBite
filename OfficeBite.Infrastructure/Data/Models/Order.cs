using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OfficeBite.Infrastructure.Data.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public DateTime SelectedDate { get; set; }
        public DateTime CurDateTime { get; set; }

        public bool IsEaten { get; set; }

        public string Details { get; set; } = string.Empty;
        public string UserAgentId { get; set; } = string.Empty;

        [ForeignKey(nameof(UserAgentId))] 
        public UserAgent UserAgent { get; set; } = null!;

        public int MenuOrderRequestNumber { get; set; }

        [ForeignKey(nameof(MenuOrderRequestNumber))]
        public MenuOrder MenuOrder { get; set; } = null!;

        public List<MenuOrder> MenuInOrder { get; set; } = new List<MenuOrder>();
    }
}
