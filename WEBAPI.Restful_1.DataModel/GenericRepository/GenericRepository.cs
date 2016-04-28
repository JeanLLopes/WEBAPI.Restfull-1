using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBAPI.Restful_1.DataModel
{
    //CRIAMOS UM REPOSITORIO GENERICO
    public class GenericRepository<TEntity> where TEntity : class 
    {
        //INTERNAL VARIABLES
        internal DB_WEBAPIEntities ContextEntities;
        internal DbSet<TEntity> DbSet;


        //CREATE THE CONTRUCTORS
        public GenericRepository(DB_WEBAPIEntities contextEntities)
        {
            ContextEntities = contextEntities;
            DbSet = contextEntities.Set<TEntity>();
        }


        //CREATE A METHODS ACCESS
        /// <summary>
        /// METHOD RETURN LIST OF DATA BASE
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> Get()
        {
            IQueryable<TEntity> query = DbSet;
            return query.ToList();
        }


        /// <summary>
        /// RETURN ELEMENT BY ID FIND
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual TEntity GetById(object id)
        {
            return DbSet.Find(id);
        }



        /// <summary>
        /// INSERT A ELEMENT IN DATABASE
        /// </summary>
        /// <param name="data"></param>
        public virtual void Insert(TEntity data)
        {
            DbSet.Add(data);
        }


        /// <summary>
        /// FIND A ELEMENT FOR A ID AND REQUEST REMOVE ON DATABASE
        /// </summary>
        /// <param name="id"></param>
        public virtual void Delete(object id)
        {
            TEntity entityToDelete = DbSet.Find(id);
            Delete(entityToDelete);
        }


        /// <summary>
        /// DELETE A ELEMENT OF DATABASE
        /// </summary>
        /// <param name="data"></param>
        public virtual void Delete(TEntity data)
        {
            if (ContextEntities.Entry(data).State == EntityState.Detached)
            {
                DbSet.Attach(data);
            }

            DbSet.Remove(data);
        }


        /// <summary>
        /// CHANGE A ELEMENT
        /// </summary>
        /// <param name="data"></param>
        public virtual void Update(TEntity data)
        {
            DbSet.Attach(data);
            ContextEntities.Entry(data).State = EntityState.Modified;
        }


        /// <summary>
        /// generic method to get many record on the basis of a condition.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> GetMany(Func<TEntity, bool> where)
        {
            return DbSet.Where(where).ToList();
        }

        /// <summary>
        /// generic method to get many record on the basis of a condition but query able.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual IQueryable<TEntity> GetManyQueryable(Func<TEntity, bool> where)
        {
            return DbSet.Where(where).AsQueryable();
        }

        /// <summary>
        /// generic get method , fetches data for the entities on the basis of condition.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public TEntity Get(Func<TEntity, Boolean> where)
        {
            return DbSet.Where(where).FirstOrDefault<TEntity>();
        }

        /// <summary>
        /// generic delete method , deletes data for the entities on the basis of condition.
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public void Delete(Func<TEntity, Boolean> where)
        {
            IQueryable<TEntity> objects = DbSet.Where<TEntity>(where).AsQueryable();
            foreach (TEntity obj in objects)
                DbSet.Remove(obj);
        }

        /// <summary>
        /// generic method to fetch all the records from db
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> GetAll()
        {
            return DbSet.ToList();
        }

        /// <summary>
        /// Inclue multiple
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="include"></param>
        /// <returns></returns>
        public IQueryable<TEntity> GetWithInclude(
            System.Linq.Expressions.Expression<Func<TEntity,
            bool>> predicate, params string[] include)
        {
            IQueryable<TEntity> query = this.DbSet;
            query = include.Aggregate(query, (current, inc) => current.Include(inc));
            return query.Where(predicate);
        }

        /// <summary>
        /// Generic method to check if entity exists
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        public bool Exists(object primaryKey)
        {
            return DbSet.Find(primaryKey) != null;
        }

        /// <summary>
        /// Gets a single record by the specified criteria (usually the unique identifier)
        /// </summary>
        /// <param name="predicate">Criteria to match on</param>
        /// <returns>A single record that matches the specified criteria</returns>
        public TEntity GetSingle(Func<TEntity, bool> predicate)
        {
            return DbSet.Single<TEntity>(predicate);
        }

        /// <summary>
        /// The first record matching the specified criteria
        /// </summary>
        /// <param name="predicate">Criteria to match on</param>
        /// <returns>A single record containing the first record matching the specified criteria</returns>
        public TEntity GetFirst(Func<TEntity, bool> predicate)
        {
            return DbSet.First<TEntity>(predicate);
        }

    }
}
