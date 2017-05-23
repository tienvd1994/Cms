using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWeb.Data
{
    public partial class EfRepository<T> : IRepository<T> where T : class
    {

        #region Fields
        /// <summary>
        /// http://www.entityframeworktutorial.net/EntityFramework4.3/dbcontext-vs-objectcontext.aspx
        /// DbContext is the primary class that is responsible for interacting (tương tác) with data as object.
        /// DbContext is responsible for the following activities:
        /// 1. EntitySet: contains entity set (DbSet<TEntity>) for all entities which is mapped to DB tables.
        /// 2. Quering: DbContext convert LINQ to Entities queries to SQL queries and send it to the database.
        /// 3. Change Tracking: It keeps track of changes that occurred in the entities after it has been querying from the database.
        /// 4. Persisting (Lưu lại) Data: It also performs the Insert, Update and Delete operations to the database, based on what the entity states.
        /// 5. Caching: DbContext does first level caching by default. It stores the entities which have been retrieved during the life time of a context class.
        /// 6. Manage Relationship: DbContext also manages relationship using CSDL, MSL and SSDL in DB-First or Model-First approach or using fluent API in Code-First approach.
        /// 7. Object Materialization: DbContext converts raw table data into entity objects.
        /// </summary>
        protected readonly DbContext dbContext;

        /// <summary>
        /// http://www.entityframeworktutorial.net/EntityFramework4.3/dbsetclass.aspx
        /// DbSet class represents an entity set that is used for create, read, update, and delete operations. A generic version of DbSet (DbSet<TEntity>)
        /// can used when the type of entity is not known at build time.
        /// </summary>
        protected readonly DbSet<T> dbSet;

        #endregion

        #region Utilities

        /// <summary>
        /// Get full error
        /// </summary>
        /// <param name="exc">Exception</param>
        /// <returns>Error</returns>
        protected string GetFullErrorText(DbEntityValidationException exc)
        {
            var msg = string.Empty;
            foreach (var validationErrors in exc.EntityValidationErrors)
                foreach (var error in validationErrors.ValidationErrors)
                    msg += string.Format("Property: {0} Error: {1}", error.PropertyName, error.ErrorMessage) + Environment.NewLine;
            return msg;
        }

        #endregion

        #region Properties
        public EfRepository(DbContext context)
        {
            dbContext = context;
            dbSet = dbContext.Set<T>();
        }

        public IQueryable<T> Table
        {
            get
            {
                return dbSet;
            }
        }

        #endregion

        public IQueryable<T> TableNoTracking
        {
            get
            {
                return dbSet.AsNoTracking();
            }
        }

        public void Delete(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                foreach (var entity in entities)
                    this.dbSet.Remove(entity);

                this.dbContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        public void Delete(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                dbSet.Remove(entity);
                dbContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        public T GetById(object id)
        {
            //see some suggested performance optimization (not tested)
            //http://stackoverflow.com/questions/11686225/dbset-find-method-ridiculously-slow-compared-to-singleordefault-on-id/11688189#comment34876113_11688189
            return this.dbSet.Find(id);
        }

        public void Insert(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                foreach (var entity in entities)
                    this.dbSet.Add(entity);

                this.dbContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        public void Insert(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                this.dbSet.Add(entity);

                this.dbContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        public void Update(IEnumerable<T> entities)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                this.dbContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }

        public void Update(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                dbContext.Entry(entity).State = EntityState.Modified;
                this.dbContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(GetFullErrorText(dbEx), dbEx);
            }
        }
    }
}
