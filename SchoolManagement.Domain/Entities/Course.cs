using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Entities
{
    [Table("Course")]
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        [Required]
        public string? CourseName { get; set; }
        public int LessonQuantity { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<UserCourse>? UserCourses { get; set; }
        public virtual ICollection<Class>? Classes { get; set; }
    }
}
