using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;

namespace ServerSide.Models.DAOs
{
	public class TicketSeatDao
	{
		private readonly AppDbContext _db;

		public TicketSeatDao(AppDbContext db)
		{
			_db = db;
		}

		public void Create(TicketSeatDto dto)
		{
			var data = ConvertToEFEntity(dto);
			_db.TicketSeats.Add(data);
			_db.SaveChanges();
		}

		private TicketSeat ConvertToEFEntity(TicketSeatDto dto)
		{
			return new TicketSeat
			{
				TicketId = dto.TicketId,
				SeatId = dto.SeatId,
			};
		}
	}
}
