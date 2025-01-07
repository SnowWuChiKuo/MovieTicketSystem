using System.ComponentModel.DataAnnotations;

namespace ServerSide.Models.ViewModels
{
    public class UserCreateVm
    {
        public int Id { get; set; }

        [Display(Name = "帳號")]
        [Required(ErrorMessage = "{0}必填")]
        [StringLength(30, ErrorMessage = "{0}長度不可超過{1}")]
        public string Account { get; set; }

        [Display(Name = "電子郵件")]
        [Required(ErrorMessage = "{0}必填")]
        [StringLength(256, ErrorMessage = "{0}長度不可超過{1}")]
        [EmailAddress(ErrorMessage = "{0}格式有誤")]
        public string Email { get; set; }

        [Display(Name = "密碼")]
        [Required(ErrorMessage = "{0}必填")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "{0}長度必須介於 {2} ~ {1}")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "名稱")]
        [StringLength(30, ErrorMessage = "{0}長度不可超過{1}")]
        public string? Name { get; set; }

        [Display(Name = "員工是否為管理員")]
        [Required(ErrorMessage = "{0}必填")]
        public bool IsAdmin { get; set; }
    }
}
