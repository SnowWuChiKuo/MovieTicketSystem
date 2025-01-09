using System.ComponentModel.DataAnnotations;

namespace ServerSide.Models.ViewModels
{
    public class CartItemVm
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "購物車Id")]
        [Required]
        public int CartId { get; set; }

        [Display(Name = "票種Id")]
        [Required]
        public int TicketId { get; set; }

        [Display(Name = "票種名稱")]
        [Required]
        public string TicketName { get; set; }

        [Display(Name = "該項票種數量")]
        [Required]
        public int Qty { get; set; }

        [Display(Name = "該項小計")]
        [Required]
        public int SubTotal { get; set; }

        [Display(Name = "創建時間")]
        [Required]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "更新時間")]
        [Required]
        public DateTime? UpdatedAt { get; set; }
    }

    public class CartItemCreateVm
    {
        public int Id { get; set; }

        [Display(Name = "購物車Id")]
        [Required]
        public int CartId { get; set; }

        [Display(Name = "票種Id")]
        [Required]
        public int TicketId { get; set; }


        [Display(Name = "該項票種數量")]
        [Required]
        public int Qty { get; set; }

        [Display(Name = "該項小計")]
        [Required]
        public int SubTotal { get; set; }
    }

    public class CartItemEditVm
    {
        public int Id { get; set; }

        [Display(Name = "購物車Id")]
        [Required]
        public int CartId { get; set; }
        [Display(Name = "票種Id")]
        [Required]
        public int TicketId { get; set; }

        [Display(Name = "票種名稱")]
        [Required]
        public string TicketName { get; set; }

        [Display(Name = "此項目單張票價")]
        [Required]
        public int TicketPrice { get; set; }

        [Display(Name = "該項票種數量")]
        [Required]
        public int Qty { get; set; }

        [Display(Name = "該項小計")]
        [Required]
        public int SubTotal { get; set; }

        [Display(Name = "創建時間")]
        [Required]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "更新時間")]
        [Required]
        public DateTime? UpdatedAt { get; set; }
    }

}
