using ServerSide.Models.DAOs;
using ServerSide.Models.DTOs;

namespace ServerSide.Models.Services
{
	public class TicketService
	{
		private readonly TicketDao _dao;

		public TicketService(TicketDao dao)
		{
			_dao = dao;
		}

		public void Create(TicketDto dto)
		{
			_dao.Create(dto);
		}
	}
}
