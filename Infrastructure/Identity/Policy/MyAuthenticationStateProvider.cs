using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using PlannerApp.Infrastructure.Identity.Models;
using System.Threading.Tasks;

namespace PlannerApp.Infrastructure.Identity.Policy
{
    public class MyAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly SignInManager<AppUser> _signInManager;        

        public MyAuthenticationStateProvider(SignInManager<AppUser> signInManager)             
        {
            _signInManager = signInManager;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {            
            var user = _signInManager.Context.User;
            return  Task.FromResult(new AuthenticationState(user));
        }
    }
}
