using System.ComponentModel.DataAnnotations;

namespace ServerSide.Models.ViewModels
{
    public class MemberIndexVm
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "帳號")]
        //[Required(ErrorMessage = "{0}必填")]
        //[StringLength(30, ErrorMessage = "{0}長度不可超過{1}")]
        public string Account { get; set; }

        [Display(Name = "電子郵件")]
        //[Required(ErrorMessage = "{0}必填")]
        //[StringLength(256, ErrorMessage = "{0}長度不可超過{1}")]
        //[EmailAddress(ErrorMessage = "{0}格式有誤")]
        public string Email { get; set; }

        [Display(Name = "密碼雜湊值")]
        //[Required(ErrorMessage = "{0}必填")]
        //[StringLength(20, MinimumLength = 6, ErrorMessage = "{0}長度必須介於 {2} ~ {1}")]
        //[DataType(DataType.Password)]
        public string PasswordHash { get; set; }

        [Display(Name = "名稱")]
        //[StringLength(30, ErrorMessage = "{0}長度不可超過{1}")]
        public string Name { get; set; }

        [Display(Name = "帳號是否刪除")]
        //[Required(ErrorMessage = "{0}必填")]
        public bool IsDeleted  { get; set; }

        [Display(Name = "帳號是否為黑名單")]
        //[Required(ErrorMessage = "{0}必填")]
        public bool IsBlackList { get; set; }

        [Display(Name = "信箱是否驗證")]
        //[Required(ErrorMessage = "{0}必填")]
        public bool IsConfirmed { get; set; }

        [Display(Name = "驗證碼")]
        //[StringLength(30, ErrorMessage = "{0}長度不可超過{1}")]
        public string ConfirmCode { get; set; }

        [Display(Name = "帳號建立時間")]
        //[Required(ErrorMessage = "{0}必填")]
        //[StringLength(30, ErrorMessage = "{0}長度不可超過{1}")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "最後更新時間")]
        //[StringLength(30, ErrorMessage = "{0}長度不可超過{1}")]
        public DateTime? UpdatedAt { get; set; }
    }
}
