using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Dtos
{
    public  class StudentGradeStatusDto
    {
        public int UserId { get; set; }
        public int ClassId { get; set; }
        public string Name { get; set; }
        public double Grade { get; set; }
        public bool IsPassed { get; set; }
    }
}
