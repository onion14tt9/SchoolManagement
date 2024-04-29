using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Dtos
{
    public class WeeklyScheduleDto
    {
        public int WeekDay { get; set; }
        public DateTime ScheduleDate { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public int SlotId { get; set; }
    }
}
