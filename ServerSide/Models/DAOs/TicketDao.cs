using Microsoft.EntityFrameworkCore;
using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.DAOs
{
	public class TicketDao
	{
		private readonly AppDbContext _db;

		public TicketDao(AppDbContext db)
		{
			_db = db;
		}

		public List<TicketVm> GetAll()
		{
            var data = _db.Tickets.Include(d => d.Screening)
                                .Select(d => new TicketVm
                                {
                                    Id = d.Id,
                                    ScreeningId = d.ScreeningId,
                                    SalesType = d.SalesType,
                                    TicketType = d.TicketType,
									ReservedSeats = d.ReservedSeats,
                                    Price = d.Price
                                }).ToList();
			return data;
        }

		public void Create(TicketDto dto)
		{
			var data = ConvertToEFEntity(dto);
			_db.Tickets.Add(data);
			_db.SaveChanges();
		}

		private Ticket ConvertToEFEntity(TicketDto dto)
		{
			return new Ticket
			{
				ScreeningId = dto.ScreeningId,
				SalesType = dto.SalesType,
				TicketType = dto.TicketType,
				ReservedSeats = dto.ReservedSeats,
                Price = dto.Price
			};
		}

		public Ticket GetTicketById(int id)
		{
			var data = _db.Tickets.FirstOrDefault(d => d.Id == id);

			if (data == null) throw new Exception("找不到此票種!");

			return data;
		}

		public TicketVm Get(int id)
		{
            var ticket = _db.Tickets.FirstOrDefault(t => t.Id == id);

			if (ticket == null)
			{
                throw new Exception("找不到此票種");
            }

			var model = new TicketVm
			{
				Id = ticket.Id,
				ScreeningId = ticket.ScreeningId,
				SalesType = ticket.SalesType,
				TicketType = ticket.TicketType,
				ReservedSeats = ticket.ReservedSeats,
				Price = ticket.Price
			};

			return model;
		}

        public void Edit(Ticket ticket)
		{
			_db.Tickets.Update(ticket);
			_db.SaveChanges();
		}

		public void Delete(Ticket ticket) 
		{
			_db.Tickets.Remove(ticket);
			_db.SaveChanges();
		}
	}
}
