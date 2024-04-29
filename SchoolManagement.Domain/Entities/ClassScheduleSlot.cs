using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Entities
{
    [Table("ClassScheduleSlot")]
    public class ClassScheduleSlot
    {
        public int ScheduleId { get; set; }
        [JsonIgnore]
        public Schedule Schedule { get; set; }
        public int SlotId { get; set; }
        [JsonIgnore]
        public Slot Slot { get; set; }
        public bool IsDeleted { get; set; }
    }
}
