using System.ComponentModel.DataAnnotations;

namespace ServerSide.Models.ViewModels
{
    public class CartVm
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "會員Id")]
        public int MemberId { get; set; }

        [Display(Name = "會員帳號")]
        public string? MemberAccount { get; set; }

        [Display(Name = "會員名字")]
        public string? MemberName { get; set; }

        [Display(Name = "物品總價")]
        public int  TotalPrice{ get; set; }

        [Display(Name = "創建時間")]
        public DateTime CreatedAt { get; set; }
    }
}
