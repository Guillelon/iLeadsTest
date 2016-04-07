using Repository.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class MasterRepository<TEntity> where TEntity : class
    {
        internal DataBaseContext context;
        internal DbSet<TEntity> dbSet;

        public MasterRepository(DataBaseContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual TEntity GetByID(int id)
        {
            return dbSet.Find(id);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return dbSet;
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }
    }
}