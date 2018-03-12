using System.Threading.Tasks;
using IdentityServer4.MongoDB.Entities;

namespace DynTech.IdentityServer.Data.Interfaces
{
    public interface IClientRepository
    {
        Task<Client> UpsertClient(Client client);
    }
}
