using System;
using System.ComponentModel.DataAnnotations;

namespace Department.Models
{
    public class Application
    {
        public long ID { get; set; }
        public long DepartID { get; set; }
        public string DName { get; set; }

        [Required(ErrorMessage = "需要填写纳新人数")]
        [Display(Name = "纳新人数")]
        public int Count { get; set; }

        [Required(ErrorMessage = "需要填写面试时间")]
        [DataType(DataType.DateTime)]
        [Display(Name = "面试时间")]
        public DateTime Time { get; set; }

        [Required(ErrorMessage = "需要选择年级")]
        [Display(Name = "年级")]
        public string Grade { get; set; }

        [Required(ErrorMessage = "需要选择学院")]
        [Display(Name = "学院")]
        public string Institute { get; set; }

        [Required(ErrorMessage = "需要填写面试地点")]
        [Display(Name = "面试地点")]
        public string Address { get; set; }

        [Required(ErrorMessage = "需要选择是否有效")]
        [Display(Name = "是否有效")]
        public bool Enabled { get; set; }
    }
}
