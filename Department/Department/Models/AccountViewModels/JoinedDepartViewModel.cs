using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Department.Models.AccountViewModels
{
    public class DNameCountDuty
    {
        public string DName { get; set; }
        public int Count { get; set; }
        public string Duty { get; set; }
    }

    public class JoinedDepartViewModel
    {
        public List<DNameCountDuty> DNameCountDuties { get; set; }
    }
}
