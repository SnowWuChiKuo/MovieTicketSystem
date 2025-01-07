using System.ComponentModel.DataAnnotations;

namespace ServerSide.Models.ViewModels
{
    public class UserIndexVm
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "帳號")]
        public string Account { get; set; }

        [Display(Name = "電子郵件")]
        public string Email { get; set; }

        [Display(Name = "密碼雜湊值")]
        public string PasswordHash { get; set; }

        [Display(Name = "名稱")]
        public string Name { get; set; }

        [Display(Name = "是否為管理員")]
        public bool IsAdmin { get; set; }

    }
}
