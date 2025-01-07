using ServerSide.Models.DAOs;
using ServerSide.Models.DTOs;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.Services
{
	public class TicketSeatService
	{
		private readonly TicketSeatDao _dao;

		public TicketSeatService(TicketSeatDao dao)
		{
			_dao = dao;
		}

		public List<TicketSeatVm> GetAll()
		{
			return _dao.GetAll();
		}

		public void Create(TicketSeatDto dto)
		{
			_dao.Create(dto);
		}

		public TicketSeatVm Get(int id)
		{
			return _dao.Get(id);
		}

		public void Edit(TicketSeatDto dto)
		{
			var ticketSeatInDb = _dao.GetTicketSeatById(dto.Id);

			if (ticketSeatInDb == null) throw new Exception("找不到此票務座位Id");

			ticketSeatInDb.SeatId = dto.SeatId;
			ticketSeatInDb.TicketId = dto.TicketId;

			_dao.Edit(ticketSeatInDb);
		}

		public void Delete(int ticketSeatId) 
		{
			var ticketSeatInDb = _dao.GetTicketSeatById(ticketSeatId);

			if (ticketSeatInDb == null) throw new Exception("找不到此票務座位Id");

			_dao.Delete(ticketSeatInDb);
		}
	}
}
