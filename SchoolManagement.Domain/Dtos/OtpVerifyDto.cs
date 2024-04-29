using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Dtos
{
    public class OtpVerifyDto
    {
        public string Email { get; set; }
        public string Otp { get; set; }
    }
}
