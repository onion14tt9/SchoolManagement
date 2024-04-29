using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Entities
{
    [Table("UserClass")]
    public class UserClass
    {
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        public int ClassId { get; set; }
        [JsonIgnore]
        public Class Class { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? GradeModifiedDate { get; set; }
        public string? GradeModifiedBy { get; set; }
        public double? Grade { get; set; }
        public bool IsDeleted { get; set; }

    }
}
