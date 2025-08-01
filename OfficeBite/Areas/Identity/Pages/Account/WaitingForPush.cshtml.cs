using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OfficeBite.Areas.Identity.Pages.Account
{
    public class WaitingForPushModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int UserId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Token { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }
    }
}
