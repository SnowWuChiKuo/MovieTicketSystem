using System.ComponentModel.DataAnnotations;

namespace ServerSide.Models.ViewModels
{
    public class CartVm
    {
        [Display(Name = "購物車Id")]
        public int Id { get; set; }

        [Display(Name = "會員Id")]
        public int MemberId { get; set; }

        [Display(Name = "會員帳號")]
        public string? MemberAccount { get; set; }

        [Display(Name = "會員名字")]
        public string? MemberName { get; set; }
    }
}
