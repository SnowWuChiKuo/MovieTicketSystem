using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClientSide.Models.ViewModels
{
    public class RegisterVm
    {
        [Display(Name = "帳號")]
        [Required(ErrorMessage = "{0}必填")]
        [StringLength(30)]
        public string Account { get; set; }

        [Display(Name = "電子郵件")]
        [Required(ErrorMessage = "{0}必填")]
        [StringLength(256)]
        public string Email { get; set; }

        [Display(Name = "密碼")]
        [Required(ErrorMessage = "{0}必填")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "{0}長度必須介於 {2} ~ {1}")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "確認密碼")]
        [Required(ErrorMessage = "{0}必填")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "{0}長度必須介於 {2} ~ {1}")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "名稱")]
        [Required(ErrorMessage = "{0}必填")]
        [StringLength(30)]
        public string Name { get; set; }

    }
}