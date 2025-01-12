using ClientSide.Models.DAOs;
using ClientSide.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientSide.Models.Services
{
	public class TicketService
	{
		public List<ScreeningChangeDateVm> GetShowTimes(int theaterId)
		{
			var dao = new TicketEFDao();
			return dao.GetShowTimes(theaterId);
		}

		public List<TicketVm> GetTicket(int screeningId)
		{
			var dao = new TicketEFDao();
			return dao.GetTicket(screeningId);
		}

		
	}
}