using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Entities
{
    [Table("Schedule")]
    public class Schedule
    {
        [Key]
        public int ScheduleId { get; set; }
        public DateTime ScheduleDate { get; set; }
        public int ClassId { get; set; }
        [JsonIgnore]
        public Class Class { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<ClassScheduleSlot>? ClassScheduleSlots { get; set; }
    }
}
