using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Dtos
{
    public class UserClassDto
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ClassId { get; set; }
    }
}
