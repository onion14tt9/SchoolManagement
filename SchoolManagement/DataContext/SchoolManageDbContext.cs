using Microsoft.EntityFrameworkCore;
using SchoolManagement.Domain.Entities;

namespace SchoolManagement.DataContext
{
    public class SchoolManageDbContext : DbContext
    {
        public SchoolManageDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<UserClass> UserClasses { get; set; }
        public DbSet<UserCourse> UserCourses { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Slot> Slots { get; set; }
        public DbSet<ClassScheduleSlot> ClassScheduleSlots { get; set; }

        public DbSet<LogEntry> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //UserCourse
            modelBuilder.Entity<UserCourse>()
                .HasKey(uc => new { uc.UserId, uc.CourseId });
            modelBuilder.Entity<UserCourse>()
                .HasOne(uc => uc.User)
                .WithMany(uc => uc.UserCourses)
                .HasForeignKey(uc => uc.UserId);
            modelBuilder.Entity<UserCourse>()
                .HasOne(uc => uc.Course)
                .WithMany(uc => uc.UserCourses)
                .HasForeignKey(uc => uc.CourseId);

            //UserClass
            modelBuilder.Entity<UserClass>()
                .HasKey(uc => new { uc.UserId, uc.ClassId });
            modelBuilder.Entity<UserClass>()
                .HasOne(uc => uc.User)
                .WithMany(uc => uc.UserClasses)
                .HasForeignKey(uc => uc.UserId);
            modelBuilder.Entity<UserClass>()
                .HasOne(uc => uc.Class)
                .WithMany(uc => uc.UserClasses)
                .HasForeignKey(uc => uc.ClassId);

            //Class

            modelBuilder.Entity<Class>()
                .HasOne(c => c.Course)
                .WithMany(uc => uc.Classes)
                .HasForeignKey(uc => uc.CourseId);

            //Schedule

            modelBuilder.Entity<Schedule>()
                .HasOne(c => c.Class)
                .WithMany(uc => uc.Schedules)
                .HasForeignKey(uc => uc.ClassId);
       

            //ClassScheduleSlot
            modelBuilder.Entity<ClassScheduleSlot>()
                .HasKey(uc => new { uc.ScheduleId, uc.SlotId });
            modelBuilder.Entity<ClassScheduleSlot>()
                .HasOne(uc => uc.Schedule)
                .WithMany(uc => uc.ClassScheduleSlots)
                .HasForeignKey(uc => uc.ScheduleId);
            modelBuilder.Entity<ClassScheduleSlot>()
                .HasOne(uc => uc.Slot)
                .WithMany(uc => uc.ClassScheduleSlots)
                .HasForeignKey(uc => uc.SlotId);

        }
    }
}