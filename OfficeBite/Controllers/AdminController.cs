using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeBite.Core.Models.AdminModels;

namespace OfficeBite.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<List<RoleViewModel>> GetRoleAsync()
        {
            return await _roleManager.Roles
                .AsNoTracking()
                .Select(c => new RoleViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
        }


        public async Task<List<UsersViewModel>> GetUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            // Създаване на списък, в който ще съхраняваме данните за потребителите
            var usersData = new List<UsersViewModel>();

            // За всеки потребител
            foreach (var user in users)
            {
                // Извличане на ролите на потребителя
                var roles = await _userManager.GetRolesAsync(user);

                // Създаване на списък за ролите на потребителя
                var roleNames = roles.ToList();

                // Добавяне на данните за потребителя и ролите му към списъка с моделите за изгледа
                usersData.Add(new UsersViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    RoleName = string.Join(", ", roleNames) // Преобразуване на списъка с роли в низ
                });
            }

            return usersData;
        }

        [HttpGet]
        public async Task<IActionResult> Admin()
        {
            var model = new AdminPanelViewModel();
            model.AllRoles = await GetRoleAsync();
            model.AllUsers = await GetUsersAsync();

            return View(model);
        }

        public async Task<IActionResult> AssignRole(AdminPanelViewModel model)
        {
            var roleId = model.RoleId;
            var userId = model.UserId;

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                return NotFound();
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles.ToArray());
            var result = await _userManager.AddToRoleAsync(user, role.Name);

            if (!result.Succeeded)
            {
                return BadRequest("HAHHAHAHHSsadasdawdas dasd asd asdsad asd asdasdasdasd");
            }
            return RedirectToAction("Admin");

        }
    }
}
