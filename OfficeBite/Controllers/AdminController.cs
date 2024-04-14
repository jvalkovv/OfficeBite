using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeBite.Core.Models.AdminModels;
using OfficeBite.Infrastructure.Data;

namespace OfficeBite.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly OfficeBiteDbContext _dbContext;


        public AdminController(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager, OfficeBiteDbContext dbContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        public async Task<List<RoleViewModel>> GetRoleAsync()
        {
            var roles = await _dbContext.Roles
                .Select(r => new RoleViewModel
                {
                    Id = r.Id,
                    Name = r.Name
                })
                .ToListAsync();

            return roles;
        }


        public async Task<List<UsersViewModel>> GetUsersAsync()
        {
            var users = _userManager.Users.ToList();
            var userInBase = _dbContext.UserAgents.ToList();
            var usersData = new List<UsersViewModel>();
            foreach (var user in users)
            {
                var currUser = userInBase.FirstOrDefault(u => u.UserId == user.Id);
                var roles = await _userManager.GetRolesAsync(user);
                var roleNames = roles.ToList();
                if (currUser != null)
                {
                    usersData.Add(new UsersViewModel
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        FullName = $"{currUser.FirstName} {currUser.LastName}",
                        Email = user.Email,
                        RoleName = string.Join(", ", roleNames)
                    });
                }
            }
            return usersData;
        }

        [HttpGet]
        public async Task<IActionResult> Admin()
        {
            var isAuthorizedAdmin = User.IsInRole("Admin");
            var isAuthorizedManager = User.IsInRole("Manager");

            if (!(isAuthorizedAdmin) && !(isAuthorizedManager))
            {
                return Unauthorized();
            }

            var model = new AdminPanelViewModel();
            model.AllRoles = await GetRoleAsync();
            model.AllUsers = await GetUsersAsync();

            return View(model);
        }

        [HttpPost]
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
                return BadRequest();
            }
            return RedirectToAction("Admin");

        }
    }
}
