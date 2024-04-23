using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeBite.Core.Models.AdminModels;
using OfficeBite.Core.Services.Contracts;
using OfficeBite.Infrastructure.Data.Common;
using OfficeBite.Infrastructure.Data.Models;

namespace OfficeBite.Core.Services
{
    public class AdminService : IAdminService

    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRepository repository;

        public AdminService(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager, IRepository _repository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            repository = _repository;
        }
        public async Task<List<RoleViewModel>> GetUserRoles()
        {
            var roles = await repository.AllReadOnly<IdentityRole>()
                .Select(r => new RoleViewModel
                {
                    Id = r.Id,
                    Name = r.Name
                })
                .ToListAsync();

            return roles;
        }

        public async Task<List<UsersViewModel>> GetUsers()
        {
            var users = _userManager.Users.ToList();
            var userInBase = repository.All<UserAgent>().ToList();
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

        public async Task<AdminPanelViewModel> Admin()
        {
           
            var model = new AdminPanelViewModel();
            model.AllRoles = await GetUserRoles();
            model.AllUsers = await GetUsers();

            return model;
        }


        public async Task AssignRole(AdminPanelViewModel model)
        {
            var roleId = model.RoleId;
            var userId = model.UserId;

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return;
            }

            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                return;
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles.ToArray());
            await _userManager.AddToRoleAsync(user, role.Name);

        }

        public Task DeleteRole(AdminPanelViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUser(AdminPanelViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
