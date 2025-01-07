using System.ComponentModel.DataAnnotations;

namespace ServerSide.Models.ViewModels
{
    public class OrderVm
    {
        [Display(Name = "訂單Id")]
        public int Id { get; set; }
        [Display(Name = "會員Id")]
        public int MemberId { get; set; }
        [Display(Name = "會員帳號")]
        public string? MemberAccount { get; set; }

        [Display(Name = "優惠碼Id")]
        public int? CouponId { get; set; }
        [Display(Name = "訂單原總金額")]
        public int TotalAmount { get; set; }
        [Display(Name = "訂單狀態")]
        public bool Status { get; set; }
        [Display(Name = "優惠後的金額")]
        public int? DiscountPrice { get; set; }
        [Display(Name = "訂單建立時間")]
        public DateTime CreatedAt { get; set; }

        // 新增的屬性
        [Display(Name = "訂單狀態")]
        public string IsStatusDisplay => Status ? "已付款" : "未付款";
    }
}
