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

		public TicketSeat GetTicketSeatById(int id)
		{
			var data = _db.TicketSeats.FirstOrDefault(d => d.Id == id);

			if (data == null) throw new Exception("找不到此票種座位Id");

			return data;
		}

		public void Edit(TicketSeat ticketSeat)
		{
			_db.TicketSeats.Update(ticketSeat);
			_db.SaveChanges();
		}

		public void Delete(TicketSeat ticketSeat) 
		{
			_db.TicketSeats.Remove(ticketSeat);
			_db.SaveChanges();
		}
	}
}
