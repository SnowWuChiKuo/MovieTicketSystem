namespace ServerSide.Models.DTOs
{
	public class TicketDto
	{
		public int Id { get; set; }

		public int ScreeningId { get; set; }

		public string SalesType { get; set; }

		public string TicketType { get; set; }

		public int ReservedSeats { get; set; }


        public int Price { get; set; }
	}
}
