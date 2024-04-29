﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Domain.Dtos
{
    public class StudentGradeDto
    {
        public int UserId { get; set; }
        public int ClassId { get; set; }
        public string? Name { get; set; }
        public double? Grade { get; set; }
    }
}
