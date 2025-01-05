using ServerSide.Models.DAOs;
using ServerSide.Models.DTOs;

namespace ServerSide.Models.Services
{
	public class TicketSeatService
	{
		private readonly TicketSeatDao _dao;

		public TicketSeatService(TicketSeatDao dao)
		{
			_dao = dao;
		}

		public void Create(TicketSeatDto dto)
		{
			_dao.Create(dto);
		}
	}
}
