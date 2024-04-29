using AutoMapper;
using SchoolManagement.Domain.Dtos;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Exceptions;
using SchoolManagement.Repositories;

namespace SchoolManagement.Services.Impl
{
    public class UserCourseService : IUserCourseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;

        public UserCourseService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContext = httpContextAccessor;
        }


        public async Task<UserCourse> AssignCourseToTeacher(UserCourseDto model)
        {
            var teacher = await _unitOfWork.UserRepository.GetAsync(model.UserId);
            if (teacher == null || teacher.Role!= UserRole.Teacher)
            {
                throw new NotFoundException("Teacher not found");
            }
            var course = await _unitOfWork.CourseRepository.GetAsync(model.CourseId) ?? throw new NotFoundException("Course not found");
            var assignment = _mapper.Map<UserCourse>(model);
            assignment.IsDeleted = false;
            var data = await _unitOfWork.UserCourseRepository.AddEntity(assignment);
            await _unitOfWork.CompleteAsync();
            return data;
        }

       

        public async Task<bool> DeleteAssignment(int userId, int courseId)
        {
            var assignment = await _unitOfWork.UserCourseRepository.GetAssignmentAsync(userId, courseId) ?? throw new NotFoundException("Assignment not found");
            assignment.IsDeleted=true;
            await _unitOfWork.UserCourseRepository.UpdateEntity(assignment);
            return true;
        }

        public async Task<StudentGradeStatusDto> DisplayCourseStatusOfStudent(int studentId, int courseId)
        {
            var lastestStudent = await _unitOfWork.UserClassRepository.GetLatestStudentGrade(studentId, courseId);
            string userName = _httpContext.HttpContext.User.Identity.Name;
            var currentUserLogin = await _unitOfWork.UserRepository.GetByUsername(userName);
            if (currentUserLogin.Role == UserRole.Teacher && await _unitOfWork.UserClassRepository.GetUserClassAsyncById(currentUserLogin.UserId, lastestStudent.ClassId) is null)
            {
                throw new ForbiddenException("You are not in class");
            }
            var status = _mapper.Map<StudentGradeStatusDto>(lastestStudent);
            status.IsPassed = lastestStudent.Grade >= 6? true : false;
            return status;
        }

        public async Task<IEnumerable<UserCourse>> GetAllAssignments()
        {
            return await _unitOfWork.UserCourseRepository.GetAllAsync();
        }

        public async Task<UserCourse?> GetAssignmentById(int userId, int courseId)
        {
            var assignment = await _unitOfWork.UserCourseRepository.GetAssignmentAsync(userId, courseId) ?? throw new NotFoundException("Assignment not found");
            return assignment;
        }

        public async Task<UserCourse> RegisterCourseForStudent(int courseId)
        {
            var course = await _unitOfWork.CourseRepository.GetAsync(courseId) ?? throw new NotFoundException("Course not found");
            string userName = _httpContext.HttpContext.User.Identity.Name;
            var currentUserLogin = await _unitOfWork.UserRepository.GetByUsername(userName);
            var userCourse = new UserCourse();
            userCourse.CourseId = courseId;
            userCourse.UserId = currentUserLogin.UserId;
            userCourse.IsDeleted = false;
            var data = await _unitOfWork.UserCourseRepository.AddEntity(userCourse);
            await _unitOfWork.CompleteAsync();
            return data;
        }

        public async Task<StudentGradeDto> UpdateGradeForStudentByCourse(UpdateStudentGradeDto request)
        {
            var lastestStudent = await _unitOfWork.UserClassRepository.GetLatestStudentGrade(request.UserId, request.CourseId);
            var userClass = await _unitOfWork.UserClassRepository.GetUserClassAsyncById(request.UserId, lastestStudent.ClassId) ?? throw new NotFoundException("Student has not attended this class yet");
            string userName = _httpContext.HttpContext.User.Identity.Name;
            var currentUserLogin = await _unitOfWork.UserRepository.GetByUsername(userName);
            if(await _unitOfWork.UserClassRepository.GetUserClassAsyncById(currentUserLogin.UserId, lastestStudent.ClassId) is null)
            {
                throw new ForbiddenException("You are not in class");
            }
            userClass.GradeModifiedDate = DateTime.Now;
            userClass.GradeModifiedBy = currentUserLogin.Name;
            userClass.Grade = request.Grade;
            await _unitOfWork.UserClassRepository.UpdateEntity(userClass);
            lastestStudent.Grade = request.Grade;
            return lastestStudent;
        }

        public async Task<bool> UpdateGradeStatusOfClass(int classId)
        {
            var studentGrades = new List<StudentGradeStatusDto>();
            var clazz = await _unitOfWork.ClassRepository.GetAsync(classId); 
            if(clazz is null) { throw new NotFoundException("Class not found"); }
            var studentIds = await _unitOfWork.UserClassRepository.GetStudentsByClassId(classId);
            foreach( var studentId in studentIds )
            {
                var userCourse = await _unitOfWork.UserCourseRepository.GetAssignmentAsync(studentId, clazz.CourseId);
                if(userCourse is null) { throw new NotFoundException("Student not in this class"); }
                var lastestStudent = await _unitOfWork.UserClassRepository.GetLatestStudentGrade(studentId, clazz.CourseId);
                //var status = await DisplayCourseStatusOfStudent(studentId, clazz.CourseId);
                userCourse.IsPassed = lastestStudent.Grade >= 6 ? true : false;
                await _unitOfWork.UserCourseRepository.UpdateEntity(userCourse);
            }
            return true;
        }

        public async Task<List<CourseDto>> ViewAllAssignedCourses()
        {
            var assignedCourses = new List<CourseDto>();
            string userName = _httpContext.HttpContext.User.Identity.Name;
            var currentUserLogin = await _unitOfWork.UserRepository.GetByUsername(userName);
            var assignedCourseIds = await _unitOfWork.UserCourseRepository.GetCoursesByUserId(currentUserLogin.UserId);
            foreach(var assignedCourseId in assignedCourseIds)
            {
                var assignedCourse = await _unitOfWork.CourseRepository.GetAsync(assignedCourseId);
                assignedCourses.Add(_mapper.Map<CourseDto>(assignedCourse));
            }
            return assignedCourses;
        }

        public async Task<StudentGradeDto> ViewGradeOfAssignedCourse(int courseId)
        {
            if(await _unitOfWork.CourseRepository.GetAsync(courseId) is null)
            {
                throw new NotFoundException("Course not found");
            }
            string userName = _httpContext.HttpContext.User.Identity.Name;
            var currentUserLogin = await _unitOfWork.UserRepository.GetByUsername(userName);
            var lastestStudent = await _unitOfWork.UserClassRepository.GetLatestStudentGrade(currentUserLogin.UserId, courseId) 
                ?? throw new NotFoundException("Student has not attended this class yet"); 
            return lastestStudent;
        }
    }
}
