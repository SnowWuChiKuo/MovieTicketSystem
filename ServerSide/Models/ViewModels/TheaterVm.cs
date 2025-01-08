using System.ComponentModel.DataAnnotations;

namespace ServerSide.Models.ViewModels
{
    public class TheaterVm
    {
        [Display(Name = "影廳編號")]
        public int Id { get; set; }

        [Display(Name = "影廳名稱")]
        public string Name { get; set; }

        [Display(Name = "總座位數")]
        public int TotalSeats { get; set; }

        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; }
    }
}
