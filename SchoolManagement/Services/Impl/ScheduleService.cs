using SchoolManagement.Domain.Dtos;
using SchoolManagement.Domain.Entities;
using SchoolManagement.Exceptions;
using SchoolManagement.Helpers;
using SchoolManagement.Repositories;

namespace SchoolManagement.Services.Impl
{
    public class ScheduleService : IScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ExcelHelper _excelHelper;

        public ScheduleService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, ExcelHelper excelHelper) 
        {
            _unitOfWork = unitOfWork;
            _httpContext = httpContextAccessor;
            _excelHelper = excelHelper;
        }
        public async Task<IEnumerable<Schedule>> AddScheduleForClassInSemester(AddClassScheduleDto request)
        {
            if(await _unitOfWork.ClassRepository.GetAsync(request.ClassId) is null) 
            {
                throw new NotFoundException("Class not found");
            }
            var schedules = new List<Schedule>();
            foreach(var schedule in request.ClassScheduleDtos)
            {
                var newSchedule = new Schedule();
                var newScheduleSlot = new ClassScheduleSlot();
                var classSchedule = new Schedule();
                classSchedule.IsDeleted = false;
                classSchedule.ClassId = request.ClassId;
                classSchedule.ScheduleDate = schedule.ScheduleDate;
                newSchedule =  await _unitOfWork.ScheduleRepository.AddEntity(classSchedule);   
                newScheduleSlot.ScheduleId = newSchedule.ScheduleId;
                newScheduleSlot.SlotId = schedule.SlotId;
                await _unitOfWork.ClassScheduleSlotRepository.AddEntity(newScheduleSlot);
                schedules.Add(newSchedule);
                for(var i = 1; i < 10; ++i)
                {
                    var data = new Schedule();
                    data.ClassId = request.ClassId;
                    data.ScheduleDate = schedule.ScheduleDate.AddDays(7*i);
                    data.IsDeleted = false;
                    newSchedule = await _unitOfWork.ScheduleRepository.AddEntity(data);
                    newScheduleSlot.ScheduleId = newSchedule.ScheduleId;
                    newScheduleSlot.SlotId = schedule.SlotId;
                    await _unitOfWork.ClassScheduleSlotRepository.AddEntity(newScheduleSlot);
                    schedules.Add(newSchedule);
                }
            }
            return schedules;

        }

        public async Task<List<WeeklyScheduleDto>> GenerateWeeklySchedule()
        {
            string userName = _httpContext.HttpContext.User.Identity.Name;
            var currentUserLogin = await _unitOfWork.UserRepository.GetByUsername(userName);
            var schedules = await _unitOfWork.ScheduleRepository.GetSchedulesByUserId(currentUserLogin.UserId);
            _excelHelper.GenerateWeeklyScheduleToExcelFile(schedules);
            return schedules;
        }

        public async Task<Schedule> GetScheduleById(int scheduleId)
        {
            return await _unitOfWork.ScheduleRepository.GetAsync(scheduleId);
        }

        public async Task<bool> RemoveSchedulesFromClass(int classId)
        {
            var schedules = await _unitOfWork.ScheduleRepository.GetSchedulesByClassId(classId);
            foreach(var schedule in schedules)
            {
                schedule.IsDeleted = true;
                await _unitOfWork.ScheduleRepository.UpdateEntity(schedule);
            }
            return true;
        }

        public async Task<List<WeeklyScheduleDto>> ViewWeeklySchedule(int weekOffset)
        {
            string userName = _httpContext.HttpContext.User.Identity.Name;
            var currentUserLogin = await _unitOfWork.UserRepository.GetByUsername(userName);
            var data = await _unitOfWork.ScheduleRepository.GetScheduleByUserIdAndWeek(currentUserLogin.UserId, weekOffset);
            return data;
        }
    }
}
