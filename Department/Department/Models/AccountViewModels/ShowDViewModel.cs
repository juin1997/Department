using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Department.Models.AccountViewModels
{
    public class ShowDViewModel
    {
        public Depart Departs { get; set; }
        [Display(Name = "是否报名")]
        public bool Enabled { get; set; }
    }
}
