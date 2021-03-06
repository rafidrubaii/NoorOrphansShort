using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoorTrust.DataAccess.SqlServer
{
    public abstract class SqlEntityFrameworkCrudRepositoryBase<TEntity, TDbContext> :
        SqlEntityFrameworkRepositoryBase<TEntity, TDbContext>, IRepository<TEntity>
        where TEntity : class, IInt32Identity
        where TDbContext : DbContext
    {
        public SqlEntityFrameworkCrudRepositoryBase(
            TDbContext context) : base(context)
        {

        }

        protected abstract DbSet<TEntity> EntityDbSet
        {
            get;
        }
        //int IRepository<TEntity>.GetActiveSponsoredOrphansCount { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        //int IRepository<TEntity>.GetActiveUnSponsoredOrphansCount { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public virtual void Delete(TEntity deleteThis)
        {
            if (deleteThis == null)
                throw new ArgumentNullException("deleteThis", "deleteThis is null.");

            var entry = Context.Entry(deleteThis);

            if (entry.State == EntityState.Detached)
            {
                EntityDbSet.Attach(deleteThis);
            }

            EntityDbSet.Remove(deleteThis);

            Context.SaveChanges();
        }

        public virtual IList<TEntity> GetAll()
        {
            return EntityDbSet.ToList();
        }
        public virtual async Task<IList<TEntity>> GetAllAsync()
        {
            return await EntityDbSet.ToListAsync();
        }

        public virtual TEntity GetById(int id)
        {
            return (
                from temp in EntityDbSet
                where temp.Id == id
                select temp
                ).FirstOrDefault();
        }
        public virtual async Task<int> CountAsync()
        {
            return await EntityDbSet.CountAsync();
        }
        public virtual async Task<int> CountActiveAsync()
        {
            return await EntityDbSet.CountAsync();
        }
        public virtual int CountActive()
        {
            return EntityDbSet.Count();
        }
     
        //public virtual async Task<IList<TEntity>> GetByIdAsync()
        //{
        //    return await EntityDbSet.ToListAsync();
        //}
        public virtual void Save(TEntity saveThis)
        {
            if (saveThis == null)
                throw new ArgumentNullException("saveThis", "saveThis is null.");

            VerifyItemIsAddedOrAttachedToDbSet(
                EntityDbSet, saveThis);

            Context.SaveChanges();
        }


    }
}
