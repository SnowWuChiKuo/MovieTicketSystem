using System.ComponentModel.DataAnnotations;

namespace ServerSide.Models.ViewModels
{
    public class PriceVm
    {
        
        [Display(Name = "票券Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "要新增票券的電影")]
        public int MovieId { get; set; }

        [Required]
        [Display(Name = "票券時段/套票(早午大夜、雙人套票...)")]
        [StringLength(30, ErrorMessage = "字數過長!")]
        public string SalesType { get; set; }

        [Required]
        [Display(Name = "票券對象、身分(全、學生、軍警...)")]
        [StringLength(30, ErrorMessage = "字數過長!")]
        public string TicketType { get; set; }

        [Required]
        [Display(Name = "票券適用人數")]
        [Range(1,2,ErrorMessage ="當前票券適用人數最多為2人!")]
        public int ReservedSeats { get; set; }

        [Required]
        [Display(Name = "票價")]
        [Range(1, 1000, ErrorMessage = "{0}範圍為{1}到{2}之間")]
        public int PriceOfTicket { get; set; }
    }

    public class PriceIndexVm
    {
        [Display(Name = "電影Id")]
        public int MovieId { get; set; }

        [Display(Name = "電影名稱")]
        public string MovieTitle { get; set; }

        [Display(Name = "票種數量")]
        public int TicketCount { get; set; }
    }


}
