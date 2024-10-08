using BLL.Services.Interfaces;
using Common.Helpers;
using DAL.Enums;
using DAL.Models.Users;
using Hangfire;
using KinopoiskWeb.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace KinopoiskWeb.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailService _emailService;

        public RegisterModel(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<RegisterModel> logger, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailService = emailService;
        }

        [BindProperty]
        public RegisterVM Input { get; set; }

        public void OnGet()
        {
            _logger.LogInformation("Navigated to the registration page.");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Attempting to register a new user.");

                var user = new User
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName
                };

                var result = await _userManager.CreateAsync(user, Input.Password);
                await _userManager.AddToRoleAsync(user, RoleType.User.ToString());
                if (result.Succeeded)
                {
                    _logger.LogInformation("User {Email} created successfully.", Input.Email);
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    BackgroundJob.Enqueue(() => _emailService.SendWelcomeEmailAsync(user));

                    return RedirectToPage("/Index");
                }

                foreach (var error in result.Errors)
                {
                    _logger.LogWarning("Error creating user {Email}: {Error}", Input.Email, error.Description);
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            _logger.LogWarning("Invalid model state while attempting to register a new user.");
            return Page();
        }
    }
}
