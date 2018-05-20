using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Department.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "需要填写邮箱")]
        [EmailAddress]
        [Display(Name = "邮箱")]
        public string Email { get; set; }

        [Required(ErrorMessage = "需要填写密码")]
        [StringLength(100, ErrorMessage = "密码长度必须在6到100字符", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Required(ErrorMessage = "需要填写确认密码")]
        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "确认密码与密码不相同")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "需要选择是社团还是学生")]
        public string Kind { get; set; }
    }
}
