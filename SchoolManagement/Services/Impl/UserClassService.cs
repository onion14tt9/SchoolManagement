using AutoMapper;
using SchoolManagement.Domain.Dtos;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Exceptions;
using SchoolManagement.Repositories;

namespace SchoolManagement.Services.Impl
{
    public class UserClassService : IUserClassService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;

        public UserClassService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) 
        { 
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContext = httpContextAccessor;
        }

        public async Task<UserClass> AcceptRegistrationOfStudent(int userId, int classId)
        {
            var userClass = await _unitOfWork.UserClassRepository.GetUserClassNotAcceptedAsyncById(userId, classId) ?? throw new NotFoundException("Registration not found");
            userClass.IsDeleted = false;
            return await _unitOfWork.UserClassRepository.UpdateEntity(userClass);
        }

        public async Task<StudentGradeDto> AddGradeForStudentByClass(AddStudentGradeDto request)
        {
            var student = await _unitOfWork.UserRepository.GetAsync(request.UserId) ?? throw new NotFoundException("Student not found");
            if(await _unitOfWork.ClassRepository.GetAsync(request.ClassId) is null)
            {
                throw new NotFoundException("Class not founnd");
            }
            var userClass = await _unitOfWork.UserClassRepository.GetUserClassAsyncById(request.UserId, request.ClassId);
            if(userClass == null) { throw new NotFoundException("Student not exist in class"); }
            userClass.Grade = request.Grade;
            userClass.GradeModifiedBy = _httpContext.HttpContext.User.Identity.Name;
            userClass.GradeModifiedDate = DateTime.Now;
            await _unitOfWork.UserClassRepository.UpdateEntity(userClass);
            var result = new StudentGradeDto();
            result.UserId = student.UserId; 
            result.ClassId = request.ClassId;
            result.Name = student.Name;
            result.Grade = request.Grade;
            return result;
        }

        public async Task<UserClass> AssignClassToUser(int userId, int classId)
        {
           var clazz = await _unitOfWork.ClassRepository.GetAsync(classId) ?? throw new NotFoundException("Class not found");
            if (await _unitOfWork.UserRepository.GetAsync(userId) is null)
            {
                throw new NotFoundException("User not found");
            }
            if (await _unitOfWork.UserCourseRepository.GetAssignmentAsync(userId, clazz.CourseId) is null)
            {
                throw new NotFoundException("Assignment not found");
            }
            var currentUserLogin = await _unitOfWork.UserRepository.GetByUsername(_httpContext.HttpContext.User.Identity.Name);
            var userClassDto = new UserClassDto();
            userClassDto.ClassId = classId;
            userClassDto.UserId = userId;
            var result = _mapper.Map<UserClass>(userClassDto);
            result.CreatedDate = DateTime.Now;
            result.CreatedBy = currentUserLogin.Username;
            result.IsDeleted = false;
            return await _unitOfWork.UserClassRepository.AddEntity(result);
        }

        public async Task<List<UserClass>> AssignUsersToClass(List<int> userIds, int classId)
        {
            var userClasses = new List<UserClass>();
            var currentUserLogin = await _unitOfWork.UserRepository.GetByUsername(_httpContext.HttpContext.User.Identity.Name);
            if (await _unitOfWork.ClassRepository.GetAsync(classId) is null) 
            {
                throw new NotFoundException("Class not found");
            }
            var userClassDto = new UserClassDto();
            userClassDto.ClassId = classId;
            foreach (int userId in userIds)
            {
                if (await _unitOfWork.UserRepository.GetAsync(userId) is null)
                {
                    throw new NotFoundException("User not found");
                }
                userClassDto.UserId = userId;
                var result = _mapper.Map<UserClass>(userClassDto);
                result.CreatedDate = DateTime.Now;
                result.CreatedBy = currentUserLogin.Username;
                result.IsDeleted = false;
                var data = await _unitOfWork.UserClassRepository.AddEntity(result);
                userClasses.Add(data);
            }
            return userClasses;
        }

        public async Task<List<StudentGradeDto>> DisplayStudentGradeInsideClass(int classId)
        {
            var currentUserLogin = await _unitOfWork.UserRepository.GetByUsername(_httpContext.HttpContext.User.Identity.Name);
            if(currentUserLogin.Role == UserRole.Teacher && await _unitOfWork.UserClassRepository.GetUserClassAsyncById(currentUserLogin.UserId,classId) is null)
            {
                throw new ForbiddenException("You are not in this class");
            }
            var studentGrades = await _unitOfWork.UserClassRepository.GetStudentGradesByClassId(classId);
            return studentGrades;
        }

        public async Task<UserClass> RegisterClassForStudent(int classId)
        {
            var clazz = await _unitOfWork.ClassRepository.GetAsync(classId) ?? throw new NotFoundException("Class not found");
            string userName = _httpContext.HttpContext.User.Identity.Name;
            var currentUserLogin = await _unitOfWork.UserRepository.GetByUsername(userName);
            if( await _unitOfWork.UserCourseRepository.GetAssignmentAsync(currentUserLogin.UserId, clazz.CourseId) is null)
            {
                throw new BadRequestException("Student hasn't registered this course");
            }
            //Update validate
            var classSchedules = await _unitOfWork.ScheduleRepository.GetSchedulesByClassId(classId);
            var userSchedules = await _unitOfWork.ScheduleRepository.GetSchedulesOfUserFromStartDateToEndDate(currentUserLogin.UserId, clazz.StartDate, clazz.DueDate);
            foreach (var userSchedule in userSchedules)
            {
                foreach(var classSchedule in classSchedules)
                {
                    var scheduleSlot = await _unitOfWork.ClassScheduleSlotRepository.GetClassScheduleSlotByScheduleId(classSchedule.ScheduleId);
                    if(await _unitOfWork.ScheduleRepository.CheckExistSlotInScheduleDate(userSchedule.ScheduleId,scheduleSlot.SlotId, userSchedule.ScheduleDate))
                    {
                        throw new BadRequestException("Cannot regist because of schedule duplication");
                    }
                }
            }
            var userClass = new UserClass();
            userClass.ClassId = classId;
            userClass.UserId = currentUserLogin.UserId;
            userClass.CreatedDate = DateTime.Now;
            userClass.CreatedBy = currentUserLogin.Username;
            userClass.IsDeleted = true;
            var data = await _unitOfWork.UserClassRepository.AddEntity(userClass);
            await _unitOfWork.CompleteAsync();
            return data;
        }

        public async Task<List<ClassDto>> ViewAllAssignedClasses()
        {
            var assignedClasses = new List<ClassDto>();
            string userName = _httpContext.HttpContext.User.Identity.Name;
            var currentUserLogin = await _unitOfWork.UserRepository.GetByUsername(userName);
            var assignedClassIds = await _unitOfWork.UserClassRepository.GetClassessByUserId(currentUserLogin.UserId);
            foreach (var assignedClassId in assignedClassIds)
            {
                var assignedClass = await _unitOfWork.ClassRepository.GetAsync(assignedClassId);
                assignedClasses.Add(_mapper.Map<ClassDto>(assignedClass));
            }
            return assignedClasses;
        }

        public async Task<List<User>> ViewStudentInsideSameAssignedClass(int classId)
        {
            var users = new List<User>();
            string userName = _httpContext.HttpContext.User.Identity.Name;
            var currentUserLogin = await _unitOfWork.UserRepository.GetByUsername(userName);
            if(await _unitOfWork.UserClassRepository.GetUserClassAsyncById(currentUserLogin.UserId, classId) is null)
            {
                throw new ForbiddenException("You are not assigned in this class");
            }
            var studentIds = await _unitOfWork.UserClassRepository.GetStudentsByClassId(classId);
            foreach (var studentId in studentIds)
            {
                users.Add(await _unitOfWork.UserRepository.GetAsync(studentId));
            }
            return users;
        }
    }
}
