using OfficeBite.Core.Models.AdminModels;

namespace OfficeBite.Core.Services.Contracts
{
    public interface IAdminService
    {
        Task<List<RoleViewModel>> GetUserRoles();

        Task<List<UsersViewModel>> GetUsers();

        Task<AdminPanelViewModel> Admin();


        Task AssignRole(AdminPanelViewModel model);

        Task DeleteRole(AdminPanelViewModel model);

        Task DeleteUser(AdminPanelViewModel model);

    }
}
