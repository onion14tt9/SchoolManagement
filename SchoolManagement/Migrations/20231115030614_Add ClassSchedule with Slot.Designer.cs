﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SchoolManagement.DataContext;

#nullable disable

namespace SchoolManagement.Migrations
{
    [DbContext(typeof(SchoolManageDbContext))]
    [Migration("20231115030614_Add ClassSchedule with Slot")]
    partial class AddClassSchedulewithSlot
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SchoolManagement.Domain.Entities.Class", b =>
                {
                    b.Property<int>("ClassId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClassId"));

                    b.Property<string>("ClassName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("ClassId");

                    b.HasIndex("CourseId");

                    b.ToTable("Class");
                });

            modelBuilder.Entity("SchoolManagement.Domain.Entities.ClassSchedule", b =>
                {
                    b.Property<int>("ClassScheduleId")
                        .HasColumnType("int");

                    b.Property<int>("ClassId")
                        .HasColumnType("int");

                    b.Property<int>("WeekDay")
                        .HasColumnType("int");

                    b.HasKey("ClassScheduleId");

                    b.ToTable("ClassSchedule");
                });

            modelBuilder.Entity("SchoolManagement.Domain.Entities.ClassScheduleSlot", b =>
                {
                    b.Property<int>("ClassScheduleId")
                        .HasColumnType("int");

                    b.Property<int>("SlotId")
                        .HasColumnType("int");

                    b.HasKey("ClassScheduleId", "SlotId");

                    b.HasIndex("SlotId");

                    b.ToTable("ClassScheduleSlot");
                });

            modelBuilder.Entity("SchoolManagement.Domain.Entities.Course", b =>
                {
                    b.Property<int>("CourseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CourseId"));

                    b.Property<string>("CourseName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("LessonQuantity")
                        .HasColumnType("int");

                    b.HasKey("CourseId");

                    b.ToTable("Course");
                });

            modelBuilder.Entity("SchoolManagement.Domain.Entities.LogEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IpAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("SchoolManagement.Domain.Entities.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ExpiredAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRevoked")
                        .HasColumnType("bit");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("IssuedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("JwtId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshToken");
                });

            modelBuilder.Entity("SchoolManagement.Domain.Entities.Slot", b =>
                {
                    b.Property<int>("SlotId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SlotId"));

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("SlotId");

                    b.ToTable("Slot");
                });

            modelBuilder.Entity("SchoolManagement.Domain.Entities.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVerified")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastModifiedPasswordDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Otp")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("OtpCreationTime")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("SchoolManagement.Domain.Entities.UserClass", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("ClassId")
                        .HasColumnType("int");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<double?>("Grade")
                        .HasColumnType("float");

                    b.Property<string>("GradeModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("GradeModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.HasKey("UserId", "ClassId");

                    b.HasIndex("ClassId");

                    b.ToTable("UserClass");
                });

            modelBuilder.Entity("SchoolManagement.Domain.Entities.UserCourse", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.HasKey("UserId", "CourseId");

                    b.HasIndex("CourseId");

                    b.ToTable("UserCourse");
                });

            modelBuilder.Entity("SchoolManagement.Domain.Entities.Class", b =>
                {
                    b.HasOne("SchoolManagement.Domain.Entities.Course", "Course")
                        .WithMany("Classes")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("SchoolManagement.Domain.Entities.ClassSchedule", b =>
                {
                    b.HasOne("SchoolManagement.Domain.Entities.Class", "Class")
                        .WithMany("ClassSchedules")
                        .HasForeignKey("ClassScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Class");
                });

            modelBuilder.Entity("SchoolManagement.Domain.Entities.ClassScheduleSlot", b =>
                {
                    b.HasOne("SchoolManagement.Domain.Entities.ClassSchedule", "ClassSchedule")
                        .WithMany("ClassSchduleSlots")
                        .HasForeignKey("ClassScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SchoolManagement.Domain.Entities.Slot", "Slot")
                        .WithMany("ClassSchduleSlots")
                        .HasForeignKey("SlotId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ClassSchedule");

                    b.Navigation("Slot");
                });

            modelBuilder.Entity("SchoolManagement.Domain.Entities.RefreshToken", b =>
                {
                    b.HasOne("SchoolManagement.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SchoolManagement.Domain.Entities.UserClass", b =>
                {
                    b.HasOne("SchoolManagement.Domain.Entities.Class", "Class")
                        .WithMany("UserClasses")
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SchoolManagement.Domain.Entities.User", "User")
                        .WithMany("UserClasses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Class");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SchoolManagement.Domain.Entities.UserCourse", b =>
                {
                    b.HasOne("SchoolManagement.Domain.Entities.Course", "Course")
                        .WithMany("UserCourses")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SchoolManagement.Domain.Entities.User", "User")
                        .WithMany("UserCourses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SchoolManagement.Domain.Entities.Class", b =>
                {
                    b.Navigation("ClassSchedules");

                    b.Navigation("UserClasses");
                });

            modelBuilder.Entity("SchoolManagement.Domain.Entities.ClassSchedule", b =>
                {
                    b.Navigation("ClassSchduleSlots");
                });

            modelBuilder.Entity("SchoolManagement.Domain.Entities.Course", b =>
                {
                    b.Navigation("Classes");

                    b.Navigation("UserCourses");
                });

            modelBuilder.Entity("SchoolManagement.Domain.Entities.Slot", b =>
                {
                    b.Navigation("ClassSchduleSlots");
                });

            modelBuilder.Entity("SchoolManagement.Domain.Entities.User", b =>
                {
                    b.Navigation("UserClasses");

                    b.Navigation("UserCourses");
                });
#pragma warning restore 612, 618
        }
    }
}
