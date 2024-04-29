using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Dtos
{
    public class CourseDto
    {
        [Required]
        public string? CourseName { get; set; }
        public int LessonQuantity { get; set; }
    }
}
