namespace SchoolManagement.Repositories
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        ITokenRepository TokenRepository { get; }
        ICourseRepository CourseRepository { get; }
        IClassRepository ClassRepository { get; }
        IUserCourseRepository UserCourseRepository { get; }
        IUserClassRepository UserClassRepository { get; }
        ISlotRepository SlotRepository { get; }
        IScheduleRepository ScheduleRepository { get; }
        IClassScheduleSlotRepository ClassScheduleSlotRepository { get; }
        Task CompleteAsync();
        /*Task Commit();
        Task Rollback();
        Task BeginTransaction();*/
    }
}
