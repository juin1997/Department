using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Department.Models
{
    public class Activity
    {
        public long ID { get; set; }
        public long DepartID { get; set; }
        public string Actaddress { get; set; }
        public DateTime Acttime { get; set; }
        public string Actintroduction { get; set; }
        public string Actpictures { get; set; }
    }
}
