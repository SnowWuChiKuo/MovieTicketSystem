using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClientSide.Models.ViewModels
{
    public class ChangePasswordVm
    {
        [Display(Name = "原始密碼")]
        [Required(ErrorMessage = "{0}必填")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "{0}長度必須介於 {2} ~ {1}")]
        [DataType(DataType.Password)]
        public string PasswordOrigin { get; set; }

        [Display(Name = "新密碼")]
        [Required(ErrorMessage = "{0}必填")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "{0}長度必須介於 {2} ~ {1}")]
        [DataType(DataType.Password)]
        public string PasswordNew { get; set; }

        [Display(Name = "確認密碼")]
        [Required(ErrorMessage = "{0}必填")]
        [StringLength(20, ErrorMessage = "{0}長度不可超過{1}")]
        [DataType(DataType.Password)]
        [Compare("PasswordNew")]
        public string ConfirmPassword { get; set; }
    }
}