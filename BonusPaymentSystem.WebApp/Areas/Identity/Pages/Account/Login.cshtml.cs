using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BonusPaymentSystem.Core.Data;
using BonusPaymentSystem.Service.Interfaces;
using BonusPaymentSystem.Core.Constants;

namespace BonusPaymentSystem.WebApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUserApplicationService _userService;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<IdentityUser> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<IdentityUser> userManager,
            IUserApplicationService userService )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _userService = userService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Usuario es campo requerido.")]
            [EmailAddress(ErrorMessage = "Favor ingresar un correo valido.")]
            [Display(Name ="Usuario")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Contraseña es campo requerido.")]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña")]
            public string Password { get; set; }

            [Display(Name = "Recordarme?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        
            if (ModelState.IsValid)
            {
                var user = _userService.Get(Input.Email);
                if (user != null && user.Status == (int) Status.INACTIVE) 
                {
                    _logger.LogWarning("Usuario inactivo, consulte con el administrador.");
                    return Page();
                }

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    /*
                    var roleStore = new RoleStore<IdentityRole>(new BpsIdentityContext("Server=LAP0301TRD028;Database=aspnet-BonusPaymentSystem.WebApp-FC57C467-C923-4625-9566-C0480FBD760A;Trusted_Connection=True;MultipleActiveResultSets=true")); //Pass the instance of your DbContext here
                    var roleManager = new RoleManager<IdentityRole>(roleStore, null, null, null, null);

                    await roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
                    await roleManager.CreateAsync(new IdentityRole { Name = "Saller" });
                    await roleManager.CreateAsync(new IdentityRole { Name = "Payer" });
                    
                    var user = await _userManager.FindByNameAsync(Input.Email);

                    await _userManager.AddToRoleAsync(user, "Admin");
                    */
                    
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("Usuario blockeado, consulte con el administrador.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Usuario y/o Contraseña incorrecta.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
