using System.ComponentModel.DataAnnotations;


namespace ServerSide.Models.ViewModels
{
    public class CartItemVm
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "購物車Id")]
        [Required(ErrorMessage = "{0}必填")]
        public int CartId { get; set; }

        [Display(Name = "票種Id")]
        [Required(ErrorMessage = "{0}必填")]
        public int TicketId { get; set; }

        [Display(Name = "票種名稱")]
        [Required(ErrorMessage = "{0}必填")]
        public string TicketName { get; set; }

        [Display(Name = "該項票種數量")]
        [Required(ErrorMessage = "{0}必填")]
        public int Qty { get; set; }

        [Display(Name = "該項小計")]
        [Required(ErrorMessage = "{0}必填")]
        public int SubTotal { get; set; }

        [Display(Name = "創建時間")]
        [Required(ErrorMessage = "{0}必填")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "更新時間")]
        [Required(ErrorMessage = "{0}必填")]
        public DateTime? UpdatedAt { get; set; }
    }

    public class CartItemCreateVm
    {
        public int Id { get; set; }

        [Display(Name = "購物車Id")]
        [Required(ErrorMessage = "{0}必填")]
        public int CartId { get; set; }

        [Display(Name = "票種Id")]
        [Required(ErrorMessage = "{0}必填")]
        public int TicketId { get; set; }


        [Display(Name = "該項票種數量")]
        [Required(ErrorMessage = "{0}必填")]
        public int Qty { get; set; }

        [Display(Name = "該項小計")]
        [Required(ErrorMessage = "{0}必填")]
        public int SubTotal { get; set; }
    }

    public class CartItemEditVm
    {
        public int Id { get; set; }

        [Display(Name = "購物車Id")]
        [Required(ErrorMessage = "{0}必填")]
        public int CartId { get; set; }

        [Display(Name = "票種Id")]
        [Required(ErrorMessage = "{0}必填")]
        public int TicketId { get; set; }

        [Display(Name = "票種名稱")]
        public string TicketName { get; set; }

        [Display(Name = "此項目單張票價")]
        public int TicketPrice { get; set; }

        [Display(Name = "該項票種數量")]
        [Required(ErrorMessage = "{0}必填")]
        public int Qty { get; set; }

        [Display(Name = "該項小計")]
        public int SubTotal { get; set; }

        [Display(Name = "創建時間")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "更新時間")]
        public DateTime? UpdatedAt { get; set; }

        public int CartItemQty { get; set; }
    }

}
