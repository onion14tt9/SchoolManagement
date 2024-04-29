using Microsoft.EntityFrameworkCore.Storage;
using SchoolManagement.DataContext;

namespace SchoolManagement.Repositories.Impl
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SchoolManageDbContext context;
        //private IDbContextTransaction transaction;

        public IUserRepository UserRepository { get; private set; }

        public ITokenRepository TokenRepository { get; private set; }

        public ICourseRepository CourseRepository { get; private set; }

        public IClassRepository ClassRepository { get; private set; }

        public IUserCourseRepository UserCourseRepository { get; private set; }

        public IUserClassRepository UserClassRepository { get; private set; }

        public ISlotRepository SlotRepository { get; private set; }

        public IScheduleRepository ScheduleRepository { get; private set; }

        public IClassScheduleSlotRepository ClassScheduleSlotRepository { get; private set; }

        public UnitOfWork(SchoolManageDbContext context
            //, IDbContextTransaction transaction
            )
        {
            this.context = context;
            UserRepository = new UserRepository(context);
            TokenRepository = new TokenRepository(context);
            CourseRepository = new CourseRepository(context);
            ClassRepository = new ClassRepository(context);
            UserCourseRepository = new UserCourseRepository(context);
            UserClassRepository = new UserClassRepository(context);
            SlotRepository = new SlotRepository(context);
            ScheduleRepository = new ScheduleRepository(context);
            ClassScheduleSlotRepository = new ClassScheduleSlotRepository(context);
        }

        public async Task CompleteAsync()
        {
            await context.SaveChangesAsync();
        }

        /*public async Task Commit()
        {
            await context.SaveChangesAsync();
            if (transaction != null)
            {
               await transaction.CommitAsync();
            }
        }

        public async Task Rollback()
        {
            if(transaction != null)
            {
                await transaction.RollbackAsync();
            }
        }

        public async Task Dispose()
        {
            if (transaction != null)
            {
                await transaction.DisposeAsync();
            }
            await context.DisposeAsync();
        }



        public async Task BeginTransaction()
        {
            transaction = await context.Database.BeginTransactionAsync();
        }*/

    }
}
