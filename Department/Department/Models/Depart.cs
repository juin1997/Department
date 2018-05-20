using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Department.Models
{
    public class Depart
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public long ID { get; set; }
        public string Name { get; set; }
        public string Minster { get; set; }
        public string Vice { get; set; }
        public string Symbol { get; set; }
    }
}
