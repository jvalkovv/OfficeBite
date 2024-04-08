using Microsoft.EntityFrameworkCore;

namespace OfficeBite.Core.Models.AdminModels
{
    public class UsersViewModel
    {

        public string UserId { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string RoleName { get; set; } = string.Empty;



    }
}
