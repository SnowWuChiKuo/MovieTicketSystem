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
        public List<TheaterVm> GetTheaters(int movieId)
        {
            var dao = new TicketEFDao();
            return dao.GetTheaters(movieId);
        }

        public List<ScreeningChangeDateVm> GetShowTimes(string theaterName, int movieId)
		{
			var dao = new TicketEFDao();
			return dao.GetShowTimes(theaterName, movieId);
		}

		public List<TicketVm> GetTicket(int screeningId)
		{
			var dao = new TicketEFDao();
			return dao.GetTicket(screeningId);
		}

		public List<SeatStatusVm> GetSeatStatus(int screeningId, string theaterName)
		{
			var dao = new TicketEFDao();
			return dao.GetSeatStatus(screeningId, theaterName);
		}


    }
}