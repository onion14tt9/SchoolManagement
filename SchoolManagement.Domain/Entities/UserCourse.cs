using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Entities
{
    [Table("UserCourse")]
    public class UserCourse
    {
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        public int CourseId { get; set; }
        [JsonIgnore]
        public Course Course { get; set; }
        public bool IsPassed { get; set; } = false;
        public bool IsDeleted { get; set; }

    }
}
