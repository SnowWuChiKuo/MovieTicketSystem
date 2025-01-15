using System.ComponentModel.DataAnnotations;

namespace ServerSide.Models.ViewModels
{
    public class OrderItemVm
    {
        [Display(Name = "訂單物品Id")]
        public int Id { get; set; }

        [Display(Name = "訂單Id")]
        [Required]
        public int OrderId { get; set; }

        [Display(Name = "票種Id")]
        [Required]
        public int TicketId { get; set; }

        [Display(Name = "票種名稱")]
        [Required]
        public string TicketName { get; set; }

        [Display(Name = "票價")]
        [Required]
        public int Price { get; set; }

        [Display(Name = "數量")]
        [Required]
        public int Qty { get; set; }

        [Display(Name = "總價")]
        [Required]
        public int SubTotal { get; set; }
		[Display(Name = "選取座位")]
		[Required]
		public string SeatNames { get; set; }
	}
}
