﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Department.Models
{
    public class Student
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public long ID { get; set; }
        public string Name { get; set; }
        public bool Gender { get; set; }
        public string Grade { get; set; }
        public string Institute { get; set; }
        public string Introduction { get; set; }
    }
}
