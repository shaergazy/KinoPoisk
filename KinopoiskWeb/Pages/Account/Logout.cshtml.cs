using DAL.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KinopoiskWeb.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        public string ReturnUrl { get; set; }


        public LogoutModel(SignInManager<User> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<IActionResult> OnPostLogoutAsync(string returnUrl = null) 
        {
            _logger.LogInformation("User {UserId} is logging out.", User.Identity?.Name);
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User {UserId} has logged out.", User.Identity?.Name);

            ReturnUrl = returnUrl ?? Url.Page("/Index");
            return LocalRedirect(ReturnUrl);
        }
    }
}
