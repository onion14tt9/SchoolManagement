using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace SchoolManagement.Domain.Entities
{
    public enum UserRole
    {
        Admin,
        Student,
        Teacher
    }
    [Table("User")]

    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        [NotNull]
        public string Name { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public UserRole Role { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsVerified {  get; set; }
        public string Otp {  get; set; }
        public DateTime OtpCreationTime { get; set; }
        public DateTime LastModifiedPasswordDate { get; set; }
        public virtual ICollection<UserCourse>? UserCourses { get; set; }
        public virtual ICollection<UserClass>? UserClasses { get; set; }
    }
}
