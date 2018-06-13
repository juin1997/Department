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
        public long ID { get; set; }
        public string Name { get; set; }
        public string Minster { get; set; }
        public string Vice { get; set; }
        public int PictureNum { get; set; }
        public string QQ { get; set; }
        public string Introduction { get; set; }
    }
}
