﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Department.Models
{
    public class DtoAMapping
    {
        public long ID { get; set; }
        public long DepartID { get; set; }
        public long ApplicationID { get; set; }
        public long StudentID { get;set; }
        public string Duty { get; set; }
        public bool Enabled { get; set; }
    }
}
