using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeBite.Core.Models.AdminModels;
using OfficeBite.Core.Services.Contracts;

namespace OfficeBite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService adminService;
        public AdminController(IAdminService adminService)
        {
            this.adminService = adminService;
        }

        [HttpGet]
        public async Task<List<RoleViewModel>> GetRoleAsync()
        {
            var roles = await adminService.GetUserRoles();

            return roles;
        }

        [HttpGet]
        public async Task<List<UsersViewModel>> GetUsersAsync()
        {
            var usersData = await adminService.GetUsers();

            return usersData;
        }

        [HttpGet]
        public async Task<IActionResult> Admin()
        {
            var result = await adminService.Admin()!;

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(AdminPanelViewModel model)
        {
            await adminService.AssignRole(model);

            return RedirectToAction(nameof(Admin));
        }
    }
}
