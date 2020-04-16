using PlannerApp.Data.ViewModels;
using PlannerApp.Infrastructure.Identity.Models;
using System.Threading.Tasks;

namespace PlannerApp.Infrastructure.Identity.Service
{
    public interface IAccountService
    {
        Task<AppUser> Login(LoginViewModel loginViewModel);
        Task<AppUser> Register(RegisterViewModel registerViewModel);
    }
}