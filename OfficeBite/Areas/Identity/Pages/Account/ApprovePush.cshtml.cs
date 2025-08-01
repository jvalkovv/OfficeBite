using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using OfficeBite.Infrastructure.Data;

namespace OfficeBite.Areas.Identity.Pages.Account
{
    public class ApprovePushModel : PageModel
    {
        private readonly OfficeBiteDbContext _db;
        private readonly IHubContext<TwoFactorHub> _hub;

        public ApprovePushModel(OfficeBiteDbContext db, IHubContext<TwoFactorHub> hub)
        {
            _db = db;
            _hub = hub;
        }

        [BindProperty(SupportsGet = true)]
        public string Token { get; set; }

        public async Task<IActionResult> OnPostApproveAsync()
        {
            var challenge = await _db.TwoFactorPushChallenges.FirstOrDefaultAsync(c => c.Token == Token);
            if (challenge == null) return NotFound();

            challenge.Status = "Approved";
            await _db.SaveChangesAsync();

            await _hub.Clients.All.SendAsync("LoginApproved", Token);
            TempData["Message"] = "Login approved successfully.";
            return RedirectToPage("/Index");
        }

        public async Task<IActionResult> OnPostDenyAsync()
        {
            var challenge = await _db.TwoFactorPushChallenges.FirstOrDefaultAsync(c => c.Token == Token);
            if (challenge == null) return NotFound();

            challenge.Status = "Denied";
            await _db.SaveChangesAsync();

            await _hub.Clients.All.SendAsync("LoginDenied", Token);
            TempData["Message"] = "Login denied.";
            return RedirectToPage("/Index");
        }
    }
}
