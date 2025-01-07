using System.ComponentModel.DataAnnotations;

namespace ServerSide.Models.ViewModels
{
    public class UserLoginVm
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "帳號")]
        [StringLength(30, ErrorMessage = "{0}長度不可超過{1}")]
        public string Account { get; set; }

        [Required(ErrorMessage = "{0}必填")]
        [Display(Name = "密碼")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "{0}長度必須介於 {2} ~ {1}")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
