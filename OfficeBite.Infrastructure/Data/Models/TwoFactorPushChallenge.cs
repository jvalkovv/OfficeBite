using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OfficeBite.Infrastructure.Data.Models
{
    public class TwoFactorPushChallenge
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
