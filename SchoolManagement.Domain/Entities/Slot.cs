using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Entities
{
    [Table("Slot")]
    public class Slot
    {
        [Key]
        public int SlotId { get; set; }
        public TimeSpan StartDate { get; set; }
        public TimeSpan DueDate { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<ClassScheduleSlot>? ClassScheduleSlots { get; set; }
    }
}
