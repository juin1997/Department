using System;
using System.ComponentModel.DataAnnotations;

namespace Department.Models
{
    public class Application
    {
        public long ID { get; set; }
        public long DepartID { get; set; }

        [Required(ErrorMessage = "需要填写人数")]
        [Display(Name = "人数")]
        public int Count { get; set; }

        [Required(ErrorMessage = "需要填写时间")]
        [DataType(DataType.DateTime)]
        [Display(Name = "时间")]
        public DateTime Time { get; set; }

        [Required(ErrorMessage = "需要选择年级")]
        [Display(Name = "年级")]
        public string Grade { get; set; }

        [Required(ErrorMessage = "需要选择学院")]
        [Display(Name = "学院")]
        public string Institute { get; set; }

        [Required(ErrorMessage = "需要填写地址")]
        [Display(Name = "地址")]
        public string Address { get; set; }

        [Required(ErrorMessage = "需要选择是否有效")]
        [Display(Name = "是否有效")]
        public bool Enabled { get; set; }
    }
}
