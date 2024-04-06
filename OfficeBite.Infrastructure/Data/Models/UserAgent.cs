using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace OfficeBite.Infrastructure.Data.Models
{
    public class UserAgent
    {
        [Comment("User identifier")]
        [Key] public string UserId { get; set; } = null!;

        [Comment("User First Name")]
        [Required]
        public string FirstName { get; set; } = string.Empty;


        [Comment("User Last Name")]
        [Required]
        public string LastName { get; set; } = string.Empty;


        [Comment("User account name")]
        [Required]
        public string Username { get; set; } = string.Empty;

        //Adding new user features if you want...

        //public string RfidCard { get; set; } = string.Empty;

        [ForeignKey(nameof(UserId))]
        public IdentityUser IdentityUser { get; set; } = null!;

        public ICollection<OrderHistory> OrderHistories { get; set; } = new List<OrderHistory>();

    }
}