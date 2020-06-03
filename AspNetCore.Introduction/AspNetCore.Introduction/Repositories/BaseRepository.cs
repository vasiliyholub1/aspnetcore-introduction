using AspNetCore.Introduction.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AspNetCore.Introduction.Repositories
{
    //Code taken from https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application.
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly AspNetCoreIntroductionContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public BaseRepository(AspNetCoreIntroductionContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        private IQueryable<TEntity> GetFilteredQuery(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            string includeProperties)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var includes = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }

            return query;
        }

        public virtual void Delete<TKey>(TKey id)
        {
            var entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }

            _dbSet.Remove(entity);
        }

        public virtual void DeleteRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Delete(entity);
            }
        }

        public virtual TEntity Find<TKey>(params TKey[] keyValues)
        {
            return _dbSet.Find(keyValues[0]);
        }

        public virtual async Task<TEntity> FindAsync<TKey>(params TKey[] keyValues)
        {
            return await _dbSet.FindAsync(keyValues[0]);
        }

        public IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            return GetFilteredQuery(filter, orderBy, includeProperties).ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            return await GetFilteredQuery(filter, orderBy, includeProperties).ToListAsync();
        }

        public TEntity GetById<TKey>(TKey id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<TEntity> GetPage(
            int pageSize,
            int pageNumber,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            var query = GetFilteredQuery(filter, orderBy, includeProperties);
            return query.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
        }

        public virtual void Insert(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Added;
        }

        /// <summary>
        /// Adds a collection of instances <see cref="TEntity"/> in data warehouse.
        /// </summary>
        /// <param name="entities">Instance collection <see cref="TEntity"/>.</param>
        public virtual void InsertRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                Insert(entity);
            }
        }

        /// <summary>
        /// Returns the number of items in the repository,
        /// matching filter.
        /// </summary>
        /// <param name="filter">Lambda expression defining filtering instance <see cref="TEntity"/>.</param>
        /// <returns>Amount of elements.</returns>
        public int ItemsCount(Expression<Func<TEntity, bool>> filter = null)
        {
            if (filter != null)
            {
                return _dbSet.Count(filter);
            }

            return _dbSet.Count();
        }

        public IQueryable<TEntity> Queryable()
        {
            return _dbSet;
        }

        /// <summary>
        /// Saves changes to the database.
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        /// <summary>
        /// Saves changes to the database asynchronously.
        /// </summary>
        /// <returns></returns>
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public virtual IQueryable<TEntity> SelectQuery<TKey>(string query, params TKey[] parameters)
        {
            return _dbSet.FromSqlRaw(query, parameters).AsQueryable();
        }

        public virtual void SetStatusAdded(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Added;
        }

        public virtual void SetStatusAddedRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                SetStatusAdded(entity);
            }
        }

        public virtual void SetStatusNotModified(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void SetStatusNotModifiedRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                SetStatusNotModified(entity);
            }
        }

        public virtual void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual void UpdateOneField<TKey>(TKey id, Expression<Func<TEntity, string>> field, string value)
        {
            // Get entity instance by id.
            var entity = _context.Entry(_dbSet.Find(id));
            if (entity != null)
            {
                // Get field name from lambda expression.
                string fieldName = ((MemberExpression)field.Body).Member.Name;
                // Set property value.
                _context.Entry(entity).Reference(fieldName).CurrentValue = value;
                // Mark property value as modified.
                _context.Entry(entity).Property(fieldName).IsModified = true;
            }
        }

        public virtual void UpdateSimpleProperties<TKey>(TEntity entity, TKey id)
        {
            _context.Entry(_dbSet.Find(id)).CurrentValues.SetValues(entity);
        }
    }
}