using AutoMapper;
using SchoolManagement.Domain.Dtos;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Exceptions;
using SchoolManagement.Repositories;

namespace SchoolManagement.Services.Impl
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CourseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Course> AddCourse(CourseDto model)
        {
            var course = _mapper.Map<Course>(model);
            course.IsDeleted = false;
            var data = await _unitOfWork.CourseRepository.AddEntity(course);
            await _unitOfWork.CompleteAsync();
            return data;
        }

        public async Task<bool> DeleteCourse(int id)
        {
            var data = await _unitOfWork.CourseRepository.GetAsync(id) ?? throw new NotFoundException("Course not found");
            data.IsDeleted = true;
            await _unitOfWork.CourseRepository.UpdateEntity(data);
            return true;
        }

        public async Task<Course?> GetCourseById(int id)
        {
            var data = await _unitOfWork.CourseRepository.GetAsync(id) ?? throw new NotFoundException("Course id not found");
            return data;
        }

        public async Task<IEnumerable<Course>> GetCourses()
        {
            return await _unitOfWork.CourseRepository.GetAllAsync();
        }

        public async Task<Course> UpdateCourse(int id, CourseDto model)
        {
            var data = await _unitOfWork.CourseRepository.GetAsync(id) ?? throw new NotFoundException("Course not found");
            data.CourseName = model.CourseName;
            data.LessonQuantity = model.LessonQuantity;
            var course = await _unitOfWork.CourseRepository.UpdateEntity(data);
            await _unitOfWork.CompleteAsync();
            return course;
        }
    }
}
