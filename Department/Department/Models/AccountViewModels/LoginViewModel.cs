using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Department.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "需要填写邮箱")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "需要填写密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "记住我?")]
        public bool RememberMe { get; set; }
    }
}
