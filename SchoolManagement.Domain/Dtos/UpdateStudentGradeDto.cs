using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Dtos
{
    public class UpdateStudentGradeDto
    {
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public double Grade { get; set; }
    }
}
