using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Dtos
{
    public class SlotDto
    {
        public TimeSpan StartDate { get; set; }
        public TimeSpan DueDate { get; set; }
    }
}
