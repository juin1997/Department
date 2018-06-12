using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Department.Models.AccountViewModels
{
    public class ApplyViewModel
    {
        public string DepartName { get; set; }
        public long DepartID { get; set; }
        public long ApplicationID { get; set; }
        public string Duty { get; set; }
        public int Count { get; set; }
        public DateTime Time { get; set; }
        public string Address { get; set; }
        public bool Enabled { get; set; }
    }
}
