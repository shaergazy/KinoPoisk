using DAL.Models.Users;
using KinopoiskWeb.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KinopoiskWeb.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<User> signInManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public LoginVM Input { get; set; }

        public string ReturnUrl { get; set; }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            _logger.LogInformation("Navigated to the login page.");
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("User {Email} attempting to log in.", Input.Email);

                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    ReturnUrl = returnUrl ?? Url.Page("/Index");
                    _logger.LogInformation("User {Email} logged in successfully.", Input.Email);
                    return LocalRedirect(ReturnUrl);
                }
                else
                {
                    _logger.LogWarning("Failed login attempt for user {Email}.", Input.Email);
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            _logger.LogWarning("Invalid model state while attempting to log in user {Email}.", Input.Email);
            return Page();
        }
    }
}
