using System.ComponentModel.DataAnnotations;

namespace ServerSide.Models.ViewModels
{
	public class TicketVm
	{
		public int Id { get; set; }

		[Required]
		[Display(Name = "場次Id")]
		public int ScreeningId { get; set; }

		[Required]
		[Display(Name = "銷售類型")]
		[StringLength(50, ErrorMessage = "銷售類型長度不可超過 50 字")]
		public required string SalesType { get; set; }

		[Required]
		[Display(Name = "票種類型")]
		[StringLength(50, ErrorMessage = "票種類型長度不可超過 50 字")]
		public required string TicketType { get; set; }

		[Required]
		[Display(Name = "價格")]
		public int Price { get; set; }
	}
}
