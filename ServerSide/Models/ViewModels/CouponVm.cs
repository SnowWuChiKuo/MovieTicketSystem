using System.ComponentModel.DataAnnotations;

namespace ServerSide.Models.ViewModels
{
    public class CouponVm
    {
        [Display(Name = "優惠碼Id")]
        public int Id { get; set; }
        [Display(Name = "優惠碼名稱")]
        public string Name { get; set; }
        [Display(Name = "優惠碼")]
        [Required]
        public string Code { get; set; }
        [Display(Name = "優惠類型")]
        [Required]
        public string DiscountType { get; set; }
        [Display(Name = "優惠數值")]
        [Required]
        public int DiscountValue { get; set; }
        [Display(Name = "到期日")]
        [Required]
        public DateOnly ExpirationDate { get; set; }
        [Display(Name = "建立日期")]
        public DateTime CreatedAt { get; set; }
        [Display(Name = "更新日期")]
        public DateTime? UpdatedAt { get; set; }

    }
}
