using AutoMapper;
using Hangfire;
using SchoolManagement.Domain.Dtos;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Exceptions;
using SchoolManagement.Repositories;

namespace SchoolManagement.Services.Impl
{
    public class ClassService : IClassService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ClassService(IUnitOfWork unitOfWork, IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Class> AddClass(int courseId, ClassDto model)
        {
            var course = await _unitOfWork.CourseRepository.GetAsync(courseId) ?? throw new NotFoundException("Course not found");
            var clazz = _mapper.Map<Class>(model);
            clazz.IsDeleted = false;
            clazz.Course = course;
            var data = await _unitOfWork.ClassRepository.AddEntity(clazz);

            BackgroundJob.Schedule<IScheduleService>(x => x.RemoveSchedulesFromClass(data.ClassId), data.DueDate);
            BackgroundJob.Schedule<IUserCourseService>(x => x.UpdateGradeStatusOfClass(data.ClassId), data.DueDate);
            return data;
        }

        public async Task<bool> DeleteClass(int id)
        {
            var data = await _unitOfWork.ClassRepository.GetAsync(id) ?? throw new NotFoundException("Class not found");
            data.IsDeleted = true;
            await _unitOfWork.ClassRepository.UpdateEntity(data);
            return true;
        }

        public async Task<Class?> GetClassById(int id)
        {
            var data = await _unitOfWork.ClassRepository.GetAsync(id) ?? throw new NotFoundException("Class id not found");
            return data;
        }

        public async Task<IEnumerable<Class>> GetClasses()
        {
            return await _unitOfWork.ClassRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Class>> GetClassesByCourse(int courseId)
        {
            return await _unitOfWork.ClassRepository.GetByCourse(courseId);
        }

        public async Task<Class> UpdateClass(int id, ClassDto model)
        {
            var data = await _unitOfWork.ClassRepository.GetAsync(id) ?? throw new NotFoundException("Class not found");
            data.ClassName = model.ClassName;
            data.StartDate = model.StartDate;
            data.DueDate = model.DueDate;
            var clazz = await _unitOfWork.ClassRepository.UpdateEntity(data);
            await _unitOfWork.CompleteAsync();
            return clazz;
        }
    }
}
