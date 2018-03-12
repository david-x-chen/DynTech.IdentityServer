using System;
using System.Threading.Tasks;
using DynTech.IdentityServer.Data.Interfaces;
using IdentityServer4.MongoDB.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace DynTech.IdentityServer.Data
{
    public class ClientRepository : MongoRepository<Client>, IClientRepository, IDisposable
    {

        private readonly MongoRepository<Client> _repository;

        private bool _disposed;

        public ClientRepository(MongoUrl mongoUrl)
        {
            _repository = new MongoRepository<Client>();
        }

        public ClientRepository(string connectionString) :
            base(connectionString)
        {
            _repository = new MongoRepository<Client>(connectionString, "Clients");

            //var pack = new ConventionPack();
            //pack.Add(new CamelCaseElementNameConvention());
            //pack.Add(new IgnoreIfNullConvention(true));

            //ConventionRegistry.Register("camel case", pack, t => true);

        }

        /// <summary>
        /// Upserts the client.
        /// </summary>
        /// <returns>The client.</returns>
        /// <param name="client">Client.</param>
        public Task<Client> UpsertClient(Client client)
        {
            ThrowIfDisposed();

            Client entity = null;

            if (client.Id != ObjectId.Empty)
            {
                var builder = Builders<Client>.Filter;
                var filter = builder.Eq(r => r.Id, client.Id);

                entity = _repository.Update(client, filter);
            }

            return Task.FromResult(entity);
        }

        public void Dispose()
        {
            _disposed = true;
        }

        /// <summary>
        /// Throws if disposed.
        /// </summary>
        /// <exception cref="System.ObjectDisposedException"></exception>
        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);
        }

    }
}
