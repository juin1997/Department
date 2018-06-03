using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Department.Models.AccountViewModels
{
    public class DNameandDuty
    {
        public string DName { get; set; }
        public string Duty { get; set; }
    }

    public class IndexSViewModel
    {
        public Student Stu { get; set; }
        public List<DNameandDuty> DNameandDuties{ get; set; }
        public List<Activity> Activities { get; set; }
    }
}
