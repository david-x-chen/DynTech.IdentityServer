using System;
using System.Threading.Tasks;
using DynTech.IdentityServer.Data.Interfaces;
using IdentityServer4.MongoDB.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace DynTech.IdentityServer.Data
{
    public class IdentityResourceRepository : MongoRepository<IdentityResource>, IIdentityResourceRepository, IDisposable
    {

        private readonly MongoRepository<IdentityResource> _repository;

        private bool _disposed;

        public IdentityResourceRepository(MongoUrl mongoUrl)
        {
            _repository = new MongoRepository<IdentityResource>();
        }

        public IdentityResourceRepository(string connectionString) :
            base(connectionString)
        {
            _repository = new MongoRepository<IdentityResource>(connectionString, "IdentityResources");

            //var pack = new ConventionPack();
            //pack.Add(new CamelCaseElementNameConvention());
            //pack.Add(new IgnoreIfNullConvention(true));

            //ConventionRegistry.Register("camel case", pack, t => true);

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