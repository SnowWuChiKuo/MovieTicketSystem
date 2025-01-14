using ClientSide.Models.Services;
using ClientSide.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientSide.Controllers
{
    public class OrdersController : Controller
    {
        private readonly OrderService _service = new OrderService();
        [Authorize]
        // GET: Orders
        public ActionResult Index()
        {
            string account = User.Identity.Name;
            try
            {
				List<OrderVm> data = _service.GetOrderInfo(account);
                return View(data);

            }
            catch (Exception ex) 
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }
    }
}