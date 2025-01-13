using ClientSide.Models.Services;
using ClientSide.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientSide.Controllers
{
    public class CartItemsController : Controller
    {
        private readonly CartItemService _service = new CartItemService();

        // GET: CartItems
        [Authorize]
        public ActionResult Detail(int id)
        {
            CartItemDetailVm vm = _service.Get(id);
            
            return View(vm);
        }
    }
}