using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PlannerApp.Data.ViewModels;
using PlannerApp.Infrastructure.Identity.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlannerApp.Infrastructure.Identity.Service
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<AccountService> _logger;

        public AccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ILogger<AccountService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<AppUser> Register(RegisterViewModel registerViewModel)
        {
            AppUser appUser = new AppUser
            {
                UserName = registerViewModel.UserName,
                Email = registerViewModel.Email
            };

            IdentityResult result = await _userManager.CreateAsync(appUser, registerViewModel.Password);

            if (result.Succeeded)
            {

                _signInManager.Context.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.NameIdentifier, appUser.UserName)
                }, "Basic"));
                //await _signInManager.SignInAsync(appUser, isPersistent: false);
                return appUser;
            }
            return null;
        }

        public async Task<AppUser> Login(LoginViewModel loginViewModel)
        {
            AppUser appUser = await _userManager.FindByNameAsync(loginViewModel.UserName);
            //AppUser appUser = await _userManager.FindByIdAsync(loginViewModel.UserName);

            if(appUser == null)
            {
                return null;
            }

            bool result = await _userManager.CheckPasswordAsync(appUser, loginViewModel.Password);

            if (result)
            {
                _signInManager.Context.User = await _signInManager.CreateUserPrincipalAsync(appUser);
                //_signInManager.Context.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                //{
                //    new Claim(ClaimTypes.NameIdentifier, appUser.UserName)
                //}, "Basic"));

                return appUser;
            }
            //SignInResult result = await _signInManager.PasswordSignInAsync(loginViewModel.UserName, loginViewModel.Password, loginViewModel.RememberMe, lockoutOnFailure: false);
            //if (result.Succeeded)
            //{
            //    _logger.LogInformation("User logged in.");
            //    return await _userManager.FindByIdAsync(loginViewModel.UserName);
            //}
            return null;
        }
    }


}
