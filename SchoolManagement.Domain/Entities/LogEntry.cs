using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Entities
{
    public class LogEntry
    {
        [Key]
        public int Id { get; set; }
        public string Action { get; set; }
        public string? Url { get; set; }
        public string IpAddress { get; set; }
        public DateTime Timestamp { get; set; }
        public string? UserId { get; set; }
    }
}
