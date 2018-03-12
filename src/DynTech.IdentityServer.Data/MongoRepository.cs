using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MongoDB.Bson;
using MongoDB.Driver;
using DynTech.IdentityServer.Data.Interfaces;

namespace DynTech.IdentityServer.Data
{
    /// <summary>
    /// Deals with entities in MongoDb.
    /// </summary>
    /// <typeparam name="T">The type contained in the repository.</typeparam>
    public class MongoRepository<T> : IRepository<T>
        where T : class
    {
        /// <summary>
        /// MongoCollection field.
        /// </summary>
        protected internal IMongoCollection<T> collection;

        /// <summary>
        /// Initializes a new instance of the MongoRepository class.
        /// Uses the Default App/Web.Config connectionstrings to fetch the connectionString and Database name.
        /// </summary>
        /// <remarks>Default constructor defaults to "MongoServerSettings" key for connectionstring.</remarks>
        public MongoRepository()
            : this(Util.GetDefaultConnectionString())
        {
        }

        /// <summary>
        /// Initializes a new instance of the MongoRepository class.
        /// </summary>
        /// <param name="connectionString">Connectionstring to use for connecting to MongoDB.</param>
        public MongoRepository(string connectionString)
        {
            this.collection = Util.GetCollectionFromConnectionString<T>(connectionString);
        }

        /// <summary>
        /// Initializes a new instance of the MongoRepository class.
        /// </summary>
        /// <param name="connectionString">Connectionstring to use for connecting to MongoDB.</param>
        /// <param name="collectionName">The name of the collection to use.</param>
        public MongoRepository(string connectionString, string collectionName)
        {
            this.collection = Util.GetCollectionFromConnectionString<T>(connectionString, collectionName);
        }

        /// <summary>
        /// Initializes a new instance of the MongoRepository class.
        /// </summary>
        /// <param name="url">Url to use for connecting to MongoDB.</param>
        public MongoRepository(MongoUrl url)
        {
            this.collection = Util.GetCollectionFromUrl<T>(url);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoRepository{T}"/> class.
        /// </summary>
        /// <param name="url">Url to use for connecting to MongoDB.</param>
        /// <param name="collectionName">The name of the collection to use.</param>
        public MongoRepository(MongoUrl url, string collectionName)
        {
            this.collection = Util.GetCollectionFromUrl<T>(url, collectionName);
        }

        /// <summary>
        /// Gets the Mongo collection (to perform advanced operations).
        /// </summary>
        /// <remarks>
        /// One can argue that exposing this property (and with that, access to it's Database property for instance
        /// (which is a "parent")) is not the responsibility of this class. Use of this property is highly discouraged;
        /// for most purposes you can use the MongoRepositoryManager&lt;T&gt;
        /// </remarks>
        /// <value>The Mongo collection (to perform advanced operations).</value>
        public IMongoCollection<T> Collection
        {
            get { return this.collection; }
        }

        public IMongoCollection<BsonDocument> GetDocumentCollection(string connectionString)
        {
            MongoUrl url = new MongoUrl(connectionString);
            return Util.GetBsonDocumentCollection(url, this.CollectionName);
        }

        /// <summary>
        /// Gets the name of the collection
        /// </summary>
        public string CollectionName
        {
            get { return this.collection.CollectionNamespace.CollectionName; }
        }

        public Type ElementType => throw new NotImplementedException();

        public Expression Expression => throw new NotImplementedException();

        public IQueryProvider Provider => throw new NotImplementedException();

        /// <summary>
        /// Returns the T by its given id.
        /// </summary>
        /// <param name="id">The Id of the entity to retrieve.</param>
        /// <returns>The Entity T.</returns>
        public virtual T GetById(ObjectId id)
        {
            return this.collection.FindSync<T>(GetIDFilter(id)).Single();
        }

        /// <summary>
        /// Adds the new entity in the repository.
        /// </summary>
        /// <param name="entity">The entity T.</param>
        /// <returns>The added entity including its new ObjectId.</returns>
        public virtual T Add(T entity)
        {
            this.collection.InsertOne(entity);

            return entity;
        }

        /// <summary>
        /// Adds the new entities in the repository.
        /// </summary>
        /// <param name="entities">The entities of type T.</param>
        public virtual void Add(IEnumerable<T> entities)
        {
            this.collection.InsertMany(entities);
        }

        /// <summary>
        /// Upserts an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>The updated entity.</returns>
        public virtual T Update(T entity, FilterDefinition<T> filter)
        {
            this.collection.ReplaceOne(filter, entity, new UpdateOptions { IsUpsert = true });
            return entity;
        }

        /// <summary>
        /// Deletes an entity from the repository by its id.
        /// </summary>
        /// <param name="id">The entity's id.</param>
        public virtual void Delete(ObjectId id)
        {
            this.collection.DeleteOne(GetIDFilter(id));
        }

        /// <summary>
        /// Deletes the entities matching the predicate.
        /// </summary>
        /// <param name="predicate">The expression.</param>
        public virtual void Delete(Expression<Func<T, bool>> predicate)
        {
            this.collection.DeleteMany<T>(predicate);
        }

        /// <summary>
        /// Deletes all entities in the repository.
        /// </summary>
        public virtual void DeleteAll()
        {
            this.collection.DeleteMany<T>(t => true);
        }

        /// <summary>
        /// Counts the total entities in the repository.
        /// </summary>
        /// <returns>Count of entities in the collection.</returns>
        public virtual long Count()
        {
            return this.collection.Count(t => true);
        }

        /// <summary>
        /// Checks if the entity exists for given predicate.
        /// </summary>
        /// <param name="predicate">The expression.</param>
        /// <returns>True when an entity matching the predicate exists, false otherwise.</returns>
        public virtual bool Exists(Expression<Func<T, bool>> predicate)
        {
            return this.collection.AsQueryable<T>().Any(predicate);
        }

        private static FilterDefinition<T> GetIDFilter(ObjectId id)
        {
            return Builders<T>.Filter.Eq("_id", id);
        }

        #region IQueryable<T>
        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An IEnumerator&lt;T&gt; object that can be used to iterate through the collection.</returns>
        public virtual IEnumerator<T> GetEnumerator()
        {
            return this.collection.AsQueryable<T>().GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An IEnumerator object that can be used to iterate through the collection.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.collection.AsQueryable<T>().GetEnumerator();
        }
        #endregion
    }
}
