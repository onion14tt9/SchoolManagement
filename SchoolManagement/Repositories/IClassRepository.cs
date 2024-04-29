using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Repositories
{
    public interface IClassRepository : IGenericRepository<Class>
    {
        Task<Class> GetByClassName(string className);
        Task<List<Class>> GetByCourse(int courseId);
    }
}
