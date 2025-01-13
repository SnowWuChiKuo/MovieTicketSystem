using ClientSide.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientSide.Controllers
{
    public class OrderItemsController : Controller
    {
        private readonly OrderItemService _service = new OrderItemService();

        [Authorize]
        // GET: OrderItems
        public ActionResult Detail(int id)
        {
            var detail = _service.Detail(id);

            return View(detail);
        }
    }
}