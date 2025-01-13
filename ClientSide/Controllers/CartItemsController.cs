using ClientSide.Models.Services;
using ClientSide.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientSide.Controllers
{
    [CartItemCleanupFilter]
    public class CartItemsController : Controller
    {
        private readonly CartItemService _service = new CartItemService();

        // GET: CartItems
        [Authorize]
        public ActionResult Detail(int? id)
        {
            if (id.HasValue)
            {
                CartItemDetailVm vm = _service.Get(id.Value);

                return View(vm);
            }
            else
            {
                return HttpNotFound();
            }
        }
    }
}