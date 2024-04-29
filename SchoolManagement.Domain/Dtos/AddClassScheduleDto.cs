using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Dtos
{
    public class AddClassScheduleDto
    {
        public int ClassId { get; set; }
        public ICollection<ClassScheduleDto>? ClassScheduleDtos { get; set; }
    }
}
