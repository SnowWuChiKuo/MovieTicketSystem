using System.ComponentModel.DataAnnotations;

namespace ServerSide.Models.ViewModels
{
    public class SeatStatusVm
    {
        [Display(Name = "場次座位狀態Id")]
        public int Id { get; set; }
        
        [Display(Name = "場次Id")]
        [Required]
        public int ScreeningId { get; set; }
        
        [Display(Name = "座位")]
        [Required]
        public int SeatId { get; set; }
        
        [Display(Name = "座位狀態")]
        [Required]
        public string Status { get; set; }

        [Display(Name = "更新時間")]
        public DateTime? UpdatedAt { get; set; }
    }
}
