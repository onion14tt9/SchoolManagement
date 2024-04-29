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
    [Table("Class")]
    public class Class
    {
        [Key]
        public int ClassId { get; set; }
        [Required]
        public string ClassName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsDeleted { get; set; }
        public int CourseId { get; set; }
        [JsonIgnore]
        public Course Course { get; set; }
        public virtual ICollection<UserClass>? UserClasses { get; set; }
        public virtual ICollection<Schedule>? Schedules { get; set; }
    }
}
