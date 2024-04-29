namespace SchoolManagement.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetAsync(int id);
        Task<T> AddEntity(T entity);
        Task<T> UpdateEntity(T entity);
        Task<bool> DeleteEntity(int id);
    }
}
