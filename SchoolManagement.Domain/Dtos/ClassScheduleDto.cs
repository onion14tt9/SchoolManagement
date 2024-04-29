using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Dtos
{
    public class ClassScheduleDto
    {
        public DateTime ScheduleDate { get; set; }
        public int SlotId { get; set; }
    }
}
