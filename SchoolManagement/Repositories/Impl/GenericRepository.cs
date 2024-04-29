using Microsoft.EntityFrameworkCore;
using SchoolManagement.DataContext;

namespace SchoolManagement.Repositories.Impl
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected SchoolManageDbContext dbContext;
        internal DbSet<T> DbSet { get; set; }
        public GenericRepository(SchoolManageDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.DbSet = this.dbContext.Set<T>();
        }
        public virtual Task<T> AddEntity(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> DeleteEntity(int id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<List<T>> GetAllAsync()
        {
            return this.DbSet.ToListAsync();
        }

        public virtual Task<T> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<T> UpdateEntity(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
