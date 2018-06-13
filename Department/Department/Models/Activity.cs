using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Department.Models
{
    public class Activity
    {
        public long ID { get; set; }
        
        [Required(ErrorMessage = "需要填写活动名称")]
        [Display(Name = "活动名称")]
        public string Name { get; set; }
        public long DepartID { get; set; }

        [Required(ErrorMessage = "需要填写活动地点")]
        [Display(Name = "活动地点")]
        public string Actaddress { get; set; }

        [Required(ErrorMessage = "需要填写活动时间")]
        [Display(Name = "活动时间")]
        public DateTime Acttime { get; set; }

        [Required(ErrorMessage = "需要填写通知时间")]
        [Display(Name = "通知时间")]
        public DateTime Noticetime { get; set; }

        [Required(ErrorMessage = "需要填写具体情况")]
        [Display(Name = "具体情况")]
        public string Actintroduction { get; set; }

        [Required(ErrorMessage = "需要选择是否有效")]
        [Display(Name = "是否有效")]
        public bool Enabled { get; set; }
    }
}
