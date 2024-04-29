using SchoolManagement.Domain.Dtos;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.Services
{
    public interface IClassService
    {
        Task<IEnumerable<Class>> GetClasses();
        Task<Class?> GetClassById(int id);
        Task<Class> AddClass(int courseId, ClassDto model);
        Task<Class> UpdateClass(int id, ClassDto model);
        Task<bool> DeleteClass(int id);
        Task<IEnumerable<Class>> GetClassesByCourse(int courseId);
    }
}
