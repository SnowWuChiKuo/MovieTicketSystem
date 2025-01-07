using ServerSide.Models.DAOs;
using ServerSide.Models.DTOs;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.Services
{
	public class TicketService
	{
		private readonly TicketDao _dao;

		public TicketService(TicketDao dao)
		{
			_dao = dao;
		}

		public List<TicketVm> GetAll()
		{
			return _dao.GetAll();
		}

		public void Create(TicketDto dto)
		{
			_dao.Create(dto);
		}

		public TicketVm Get(int id)
		{
			return _dao.Get(id);
		}

		public void Edit(TicketDto dto)
		{
			var ticketInDb = _dao.GetTicketById(dto.Id);

			if (ticketInDb == null)
			{
				throw new Exception("找不到此票種!");
			}

			ticketInDb.ScreeningId = dto.ScreeningId;
			ticketInDb.SalesType = dto.SalesType;
			ticketInDb.TicketType = dto.TicketType;
			ticketInDb.Price = dto.Price;

			_dao.Edit(ticketInDb);
		}

		public void Delete(int ticketId) 
		{
			var ticket = _dao.GetTicketById(ticketId);
			if (ticket == null) 
			{
				throw new Exception("找不到此票種!");
			}

			_dao.Delete(ticket);
		}
	}
}
