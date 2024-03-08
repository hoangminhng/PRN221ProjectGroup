using ChatSystem.Hubs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

namespace ChatSystem.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly IHubContext<OnlineHub> _hubContext; // Add this field

        public LogoutModel(IHubContext<OnlineHub> hubContext)
        {
            _hubContext = hubContext; // Initialize hub context
        }
        public async Task<IActionResult> OnPostAsync()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            await _hubContext.Clients.All.SendAsync("UserDisconnected", HttpContext.Session.GetInt32("UserId"));
            return RedirectToPage("/Account/Login");
        }
    }
}
