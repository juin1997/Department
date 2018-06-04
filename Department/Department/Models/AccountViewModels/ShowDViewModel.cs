using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Department.Models.AccountViewModels
{
    public class ShowDViewModel
    {
        public Depart Department { get; set; }
        public long Sid { get; set; }
        public long Did { get; set; }
        public long Aid { get; set; }
        [Display(Name = "是否报名")]
        public bool Enabled { get; set; }
    }
}
