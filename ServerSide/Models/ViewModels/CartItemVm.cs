using System.ComponentModel.DataAnnotations;

namespace ServerSide.Models.ViewModels
{
    public class CartItemVm
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

        [Display(Name = "該項票種數量")]
        [Required]
        public int Qty { get; set; }

    }
}
