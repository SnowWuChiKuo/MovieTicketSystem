using ClientSide.Models.EFModels;
using ClientSide.Models.Services;
using ClientSide.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientSide.Controllers
{
	[Route("Tickets")]
	public class TicketsController : Controller
    {
		// GET: Tickets
		public ActionResult Index()
        {
            var service = new TicketService();
			return View();
        }

        [HttpPost]
        public JsonResult GetTheaters(int? movieId)
        {
            try
            {
                if (!movieId.HasValue)
                {
                    return Json(new { success = false, message = "影廳編號不可為空" });
                }

                var service = new TicketService();
                var showtime = service.GetTheaters(movieId.Value);

                return Json(new { success = true, data = showtime }, JsonRequestBehavior.AllowGet); // 加上 JsonRequestBehavior
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
		public JsonResult GetShowTimes(string theaterName, int movieId)
		{
			try
			{
				var service = new TicketService();
				var showtime = service.GetShowTimes(theaterName, movieId);

				return Json(new { success = true, data = showtime }, JsonRequestBehavior.AllowGet); // 加上 JsonRequestBehavior
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = ex.Message });
			}
		}		

		[HttpPost]
		public JsonResult GetTicket(int? screeningId)
		{
			try
			{
				if (!screeningId.HasValue)
				{
					return Json(new { success = false, message = "場次編號不可為空" });
				}

				var service = new TicketService();
				var ticket = service.GetTicket(screeningId.Value);

				return Json(new { success = true, data = ticket });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = ex.Message });
			}
		}

		[HttpPost]
		public JsonResult GetSeatStatus(int? screeningId)
		{
			try
			{
				if (!screeningId.HasValue)
				{
					return Json(new { success = false, message = "場次編號不可為空" });
				}

				var service = new TicketService();
				var seat = service.GetSeatStatus(screeningId.Value);

				return Json(new { success = true, data = seat });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = ex.Message });
			}
		}

		
	}
}