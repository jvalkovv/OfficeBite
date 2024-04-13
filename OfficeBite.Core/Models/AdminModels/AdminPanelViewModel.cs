namespace OfficeBite.Core.Models.AdminModels
{
    public class AdminPanelViewModel
    {
        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string RoleName { get; set; } = string.Empty;

        public string RoleId { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public List<RoleViewModel> AllRoles { get; set; } = new List<RoleViewModel>();

        public List<UsersViewModel> AllUsers { get; set; } = new List<UsersViewModel>();
    }
}
