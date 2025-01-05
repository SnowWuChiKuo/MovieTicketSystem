using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClientSide.Models.ViewModels
{
    public class ProfileVm
    {
        [Display(Name = "帳號")]
        public string Account { get; set; }

        [Display(Name = "電子郵件")]
        [Required(ErrorMessage = "{0}必填")]
        [EmailAddress(ErrorMessage = "{0}格式有誤")]
        [StringLength(256, ErrorMessage = "{0}長度不可超過{1}")]
        public string Email { get; set; }

        [Display(Name = "名稱")]
        [Required(ErrorMessage = "{0}必填")]
        [StringLength(30, ErrorMessage = "{0}長度不可超過{1}")]
        public string Name { get; set; }
    }
}
