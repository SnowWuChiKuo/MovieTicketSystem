using Microsoft.EntityFrameworkCore;
using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.DAOs
{
	public class TicketSeatDao
	{
		private readonly AppDbContext _db;

		public TicketSeatDao(AppDbContext db)
		{
			_db = db;
		}

		public List<TicketSeatVm> GetAll()
		{
            var data = _db.TicketSeats.Include(d => d.Seat)
										.Include(d => d.Ticket)
										.Select(d => new TicketSeatVm
										{
											Id = d.Id,
											SeatId = d.SeatId,
											TicketId = d.TicketId,
										}).ToList();
			return data;
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

		public TicketSeatVm Get(int id)
		{
			var ticketSeat = _db.TicketSeats.AsNoTracking()
												.Include(d => d.Ticket)
												.Include(d => d.Seat)
												.FirstOrDefault(d => d.Id == id);
			if (ticketSeat == null) throw new Exception("找不到此票種座位Id");

            var model = new TicketSeatVm
			{
				Id = ticketSeat.Id,
				TicketId = ticketSeat.TicketId,
				SeatId = ticketSeat.SeatId,
			};
			return model;

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
