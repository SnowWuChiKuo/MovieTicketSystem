using Microsoft.AspNetCore.Mvc.Rendering;
using MovieTicketSystem.Models.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerSide.Models.ViewModels
{
    public class ScreeningVm
    {
        [Display(Name = "場次編號")]
        public int Id { get; set; }

        [Display(Name = "電影Id")]
        public int MovieId { get; set; }

        [Display(Name = "電影名稱")]
        public string MovieTitle { get; set; }

        [Display(Name = "該場影廳編號")]
        public int TheaterId { get; set; }

        [Display(Name = "場次開始日")]
        public DateOnly Televising { get; set; }

        [Display(Name = "場次開始時間")]
        public TimeOnly StartTime { get; set; }

        [Display(Name = "場次結束時間")]
        public TimeOnly EndTime { get; set; }

        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "最後更新")]
        public DateTime? UpdatedAt { get; set; }
    }


    public class ScreeningEditVm
    {
        [Display(Name = "場次編號")]
        public int Id { get; set; }

        [Display(Name = "電影Id")]
        public int MovieId { get; set; }

        [Display(Name = "電影名稱")]
        public string MovieTitle { get; set; }

        [Display(Name = "該場影廳編號")]
        [Required(ErrorMessage = "請選擇影廳")]
        public int TheaterId { get; set; }

        [Display(Name = "場次開始日")]
        [Required(ErrorMessage = "請選擇日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [ValidScreeningDate]
        public DateOnly Televising { get; set; }

        [Display(Name = "場次開始時間")]
        public TimeOnly StartTime { get; set; }

        [Display(Name = "片長")]
        public int? RunTime { get; set; }
        [Display(Name = "場次結束時間")]
        public TimeOnly EndTime { get; set; }

		[NotMapped]
		//電影的下拉清單屬性
		public List<SelectListItem> MovieOptions { get; set; } = new List<SelectListItem>();

        [NotMapped]
        //影廳的下拉選單
        public List<SelectListItem> TheaterOptions { get; set; } = new List<SelectListItem>();

    }
}
