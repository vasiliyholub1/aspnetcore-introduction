using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AspNetCore.Introduction.Interfaces
{
    /// <summary>
    /// Represents a generic repository
    /// </summary>
    /// <typeparam name="TEntity">Type whose instances the repository manages</typeparam>
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Returns an enumeration of instances <see cref="TEntity"/> from the data store.
        /// Applies a filter, sorting, and loading related properties
        /// if appropriate parameter values are specified
        /// </summary>
        /// <param name="filter">Lambda expression defining filtering of instances <see cref="TEntity"/></param>
        /// <param name="orderBy">Lambda expression specifying sorting of instances <see cref="TEntity"/></param>
        /// <param name="includeProperties">List of related instance properties <see cref="TEntity"/>, comma separated</param>
        /// <returns>Listing instances <see cref="TEntity"/></returns>
        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        /// <summary>
        /// Returns an enumeration of instances <see cref="TEntity"/> from the data store asynchronously.
        /// Applies a filter, sorting, and loading related properties
        /// if appropriate parameter values are specified
        /// </summary>
        /// <param name="filter">Lambda expression defining filtering of instances <see cref="TEntity"/></param>
        /// <param name="orderBy">Lambda expression specifying sorting of instances <see cref="TEntity"/></param>
        /// <param name="includeProperties">List of related instance properties <see cref="TEntity"/>, comma separated</param>
        /// <returns>Listing instances <see cref="TEntity"/></returns>
        Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        /// <summary>
        /// Returns the page of the given size with the given number
        /// as a listing of instances <see cref="TEntity"/> from data store.
        /// Applies filtering, sorting, and loading related properties,
        /// if appropriate parameter values are specified
        /// </summary>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="filter">Lambda expression defining filtering of instances <see cref="TEntity"/></param>
        /// <param name="orderBy">Lambda expression specifying sorting of instance <see cref="TEntity"/></param>
        /// <param name="includeProperties">List of related instance properties <see cref="TEntity"/>, comma separated</param>
        /// <returns>Listing instances.<see cref="TEntity"/></returns>
        IEnumerable<TEntity> GetPage(
            int pageSize,
            int pageNumber,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        /// <summary>
        /// Returns instance <see cref="TEntity"/>,
        /// corresponding to the given identifier from the data store.
        /// </summary>
        /// <typeparam name="TKey">ID Type.</typeparam>
        /// <param name="id">Instance id.</param>
        /// <returns>Instance <see cref="TEntity"/>.</returns>
        TEntity GetById<TKey>(TKey id);

        void SetStatusAdded(TEntity entity);

        void SetStatusAddedRange(IEnumerable<TEntity> entities);

        void SetStatusNotModified(TEntity entity);

        void SetStatusNotModifiedRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Adds the specified instance <see cref="TEntity"/> to the data warehouse.
        /// </summary>
        /// <param name="entity">Item instance.<see cref="TEntity"/>.</param>
        void Insert(TEntity entity);

        /// <summary>
        /// Updates the given instance of <see cref="TEntity"/> in the data warehouse.
        /// </summary>
        /// <param name="entity">Entity instance.<see cref="TEntity"/></param>
        void Update(TEntity entity);

        /// <summary>
        /// Updates an instance of <see cref="TEntity"/> with the specified identifier <see cref="TKey"/>
        /// in a POCO model.
        /// </summary>
        /// <param name="entity">Instance <see cref="TEntity"/>.</param>
        /// <param name="id">Instance id <see cref="TEntity"/>.</param>
        void UpdateSimpleProperties<TKey>(TEntity entity, TKey id);

        /// <summary>
        /// Updates only one entity field <see cref="TEntity"/> with the specified identifier <see cref="TKey"/>
        /// </summary>
        /// <param name="id">Instance id.<see cref="TEntity"/>.</param>
        /// <param name="field">Lambda expression defining a field name.<see cref="TEntity"/>.</param>
        /// <param name="value">Updated value.<see cref="TEntity"/>.</param>
        void UpdateOneField<TKey>(TKey id, Expression<Func<TEntity, string>> field, string value);

        /// <summary>
        /// Deletes an instance <see cref="TEntity"/>,
        ///corresponding to the given identifier from the data store.
        /// </summary>
        /// <typeparam name="TKey">ID Type.</typeparam>
        /// <param name="id">Instance id.</param>
        void Delete<TKey>(TKey id);

        /// <summary>
        /// Deletes the specified instance of <see cref="TEntity"/> Deletes the specified instance of
        /// </summary>
        /// <param name="entity">Instance instance<see cref="TEntity"/></param>
        void Delete(TEntity entity);

        int ItemsCount(Expression<Func<TEntity, bool>> filter = null);

        TEntity Find<TKey>(params TKey[] keyValues);
        Task<TEntity> FindAsync<TKey>(params TKey[] keyValues);

        IQueryable<TEntity> SelectQuery<TKey>(string query, params TKey[] parameters);

        void InsertRange(IEnumerable<TEntity> entities);

        IQueryable<TEntity> Queryable();

        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
