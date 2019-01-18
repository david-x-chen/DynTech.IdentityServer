
using System.Threading.Tasks;

namespace DynTech.IdentityServer.Data.Interfaces
{
    public interface IIdentityResourceRepository
    {
        Task GetResources();
    }
}