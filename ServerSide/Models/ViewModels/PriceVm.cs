using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerSide.Models.ViewModels
{
    public class PriceVm
    {
        
        [Display(Name = "票券Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "要新增票券的電影")]
        public int MovieId { get; set; }

        // 定義固定選項
        public static readonly List<string> SalesTypeOptions = new List<string>
        {
            "早鳥優惠",
            "大夜票",
            "平日票"
        };

        public static readonly List<string> TicketTypeOptions = new List<string>
        {
            "全票",
            "學生票",
            "軍警票",
            "雙人套票"
        };

        [Required]
        [Display(Name = "票券時段/套票")]
        public string SalesType { get; set; }

		[NotMapped]
		public List<SelectListItem> SalesTypeItem { get; set; } = new List<SelectListItem>();

		[NotMapped]
		public List<SelectListItem> TicketTypeOption { get; set; } = new List<SelectListItem>();

		[Required]
        [Display(Name = "票券對象、身分")]
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
