using System.Security.Policy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OfficeBite.Infrastructure.Data;

namespace OfficeBite.Areas.Identity.Pages.Account
{
    public class VerifyPushModel : PageModel
    {
        private readonly OfficeBiteDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public VerifyPushModel(OfficeBiteDbContext db, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGetAsync(int userId, string token, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            // Validate token approval
            var challenge = await _db.TwoFactorPushChallenges.FirstOrDefaultAsync(c => c.Token == token && c.UserId == userId);
            if (challenge == null || challenge.Status != "Approved")
                return RedirectToPage("./Login");

            // Get user
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return RedirectToPage("./Login");

            // Complete login
            await _signInManager.SignInAsync(user, isPersistent: false);
            return LocalRedirect(returnUrl);
        }
    }
}
