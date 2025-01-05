using System.ComponentModel.DataAnnotations;

namespace ServerSide.Models.ViewModels
{
	public class GenreVm
	{
		public int Id { get; set; }


		[Display(Name = "分類名稱")]
		[Required(ErrorMessage = "請填入{0}")]
		public string Name { get; set; }


		[Display(Name = "顯示順序")]
		[Required(ErrorMessage = "請填入{0}")]
		public int DisplayOrder { get; set; }
	}
}
