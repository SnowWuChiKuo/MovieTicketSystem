using System.ComponentModel.DataAnnotations;

namespace ServerSide.Models.ViewModels
{
    public class ReviewVm
    {
        public int Id { get; set; }

        [Display(Name="會員Id")]
        public int MemberId { get; set; }

        [Display(Name = "會員名稱")]
        public string MemberName { get; set; }

        [Display(Name = "電影Id")]
        public int MovieId { get; set; }

        [Display(Name = "電影名稱")]
        public string MovieTitle { get; set; }

        [Display(Name = "觀影訂單Id")]
        public int OrderId { get; set; }

        [Display(Name = "評級")]
        [Required(ErrorMessage ="請至少填寫{0}")]
        [Range(1, 5, ErrorMessage = "{0}必須介於{1}到{2}之間")]
        public int Rating { get; set; }

        [Display(Name = "評論內容")]
        [StringLength(2000)]
        public string? Comment { get; set; }

        [Display(Name = "建檔時間")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "最後更新")]
        public DateTime? UpdatedAt { get; set; }

    }
}
