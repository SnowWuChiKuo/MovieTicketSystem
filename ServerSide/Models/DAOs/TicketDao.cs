using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;

namespace ServerSide.Models.DAOs
{
	public class TicketDao
	{
		private readonly AppDbContext _db;

		public TicketDao(AppDbContext db)
		{
			_db = db;
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
				Price = dto.Price
			};
		}

		public Ticket GetTicketById(int id)
		{
			var data = _db.Tickets.FirstOrDefault(d => d.Id == id);

			if (data == null) throw new Exception("找不到此票種!");

			return data;
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
