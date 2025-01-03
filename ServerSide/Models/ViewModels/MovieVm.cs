using ServerSide.Models.EFModels;
using System.ComponentModel.DataAnnotations;

namespace ServerSide.Models.ViewModels
{
    public class MovieVm
    {
        public int Id { get; set; }


        [Display(Name = "分類")]
        [Required(ErrorMessage = "請填入{0}")]
        public int GenreId { get; set; }


        [Display(Name = "分級")]
        [Required(ErrorMessage = "請填入{0}")]
        public int RatingId { get; set; }


        [Display(Name = "片名")]
        [Required(ErrorMessage = "請填入{0}")]
        [StringLength(70)]
        public string Title { get; set; }


        [Display(Name = "敘述")]
        [Required(ErrorMessage = "請填入{0}")]
        [StringLength(2000)]
        public string Description { get; set; }


        [Display(Name = "導演")]
        [Required(ErrorMessage = "請填入{0}")]
        [StringLength(100)]
        public string Director { get; set; }


        [Display(Name = "演員")]
        [Required(ErrorMessage = "請填入{0}")]
        [StringLength(1000)]
        public string Cast { get; set; }


        [Display(Name = "片長")]
        [Range(1,500 , ErrorMessage = "{0}必須介於{1}到{2}之間")]
        
        public int? RunTime { get; set; }


        [Display(Name = "上檔日")]
        [Required(ErrorMessage = "請填入{0}")]
        public DateTime ReleaseDate { get; set; }


        [Display(Name = "海報連結")]
        [StringLength(70)]
        public string? PosterUrl { get; set; }


        [Display(Name = "預告連結")]
        [StringLength(70)]
        public string? TrailerUrl { get; set; }


        [Display(Name = "建檔時間")]
        public DateTime CreatedAt { get; set; }


        [Display(Name = "最後更新")]
        public DateTime? Updated { get; set; }
    }
}
