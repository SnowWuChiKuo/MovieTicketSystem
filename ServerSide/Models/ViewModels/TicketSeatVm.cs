using System.ComponentModel.DataAnnotations;

namespace ServerSide.Models.ViewModels
{
	public class TicketSeatVm
	{
		public int Id { get; set; }

		[Required]
		[Display(Name = "票種Id")]
		public int TicketId { get; set; }

		[Required]
		[Display(Name = "座位Id")]
		public int SeatId { get; set; }
	}
}
