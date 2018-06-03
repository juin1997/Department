using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Department.Models
{
    public class DtoMMapping
    {
        public long ID { get; set; }
        public long DepartID { get; set; }
        public string DepartName { get; set; }
        public string Duty { get; set; }
        public long MemberID { get; set; }
    }
}
