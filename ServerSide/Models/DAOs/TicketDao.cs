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
	}
}
