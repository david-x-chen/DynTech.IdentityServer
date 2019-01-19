
using System.Threading.Tasks;
using DynTech.IdentityServer.Models.AccountViewModels;

namespace DynTech.IdentityServer.Services
{
    public interface IAccountService
    {
        Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId);

        Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId);
    }
}