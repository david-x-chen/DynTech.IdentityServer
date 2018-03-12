using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DynTech.IdentityServer.Data.Interfaces
{
    /// <summary>
    /// IRepository definition.
    /// </summary>
    /// <typeparam name="T">The type contained in the repository.</typeparam>
    public interface IRepository<T> : IQueryable<T>
        where T : class
    {
        /// <summary>
        /// Gets the Mongo collection (to perform advanced operations).
        /// </summary>
        /// <remarks>
        /// One can argue that exposing this property (and with that, access to it's Database property for instance
        /// (which is a "parent")) is not the responsibility of this class. Use of this property is highly discouraged;
        /// for most purposes you can use the MongoRepositoryManager&lt;T&gt;
        /// </remarks>
        /// <value>The Mongo collection (to perform advanced operations).</value>
        IMongoCollection<T> Collection { get; }

        /// <summary>
        /// Returns the T by its given id.
        /// </summary>
        /// <param name="id">The value representing the ObjectId of the entity to retrieve.</param>
        /// <returns>The Entity T.</returns>
        T GetById(ObjectId id);

        /// <summary>
        /// Adds the new entity in the repository.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The added entity including its new ObjectId.</returns>
        T Add(T entity);

        /// <summary>
        /// Adds the new entities in the repository.
        /// </summary>
        /// <param name="entities">The entities of type T.</param>
        void Add(IEnumerable<T> entities);

        /// <summary>
        /// Upserts an entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="filter">The filter.</param>
        /// <returns>The updated entity.</returns>
        T Update(T entity, FilterDefinition<T> filter);

        /// <summary>
        /// Deletes an entity from the repository by its id.
        /// </summary>
        /// <param name="id">The entity's id.</param>
        void Delete(ObjectId id);

        /// <summary>
        /// Deletes the entities matching the predicate.
        /// </summary>
        /// <param name="predicate">The expression.</param>
        void Delete(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Deletes all entities in the repository.
        /// </summary>
        void DeleteAll();

        /// <summary>
        /// Counts the total entities in the repository.
        /// </summary>
        /// <returns>Count of entities in the repository.</returns>
        long Count();

        /// <summary>
        /// Checks if the entity exists for given predicate.
        /// </summary>
        /// <param name="predicate">The expression.</param>
        /// <returns>True when an entity matching the predicate exists, false otherwise.</returns>
        bool Exists(Expression<Func<T, bool>> predicate);
    }
}
