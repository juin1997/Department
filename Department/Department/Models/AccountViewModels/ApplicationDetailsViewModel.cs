﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Department.Models.AccountViewModels
{
    public class ApplicationDetailsViewModel
    {
        public Student Stu { get; set; }
        // 部门ID
        public long Did { get; set; }
        // 学生ID
        public long Sid { get; set; }
    }
}
