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

        [Authorize]
        public ActionResult DeleteCartItem(int? id)
        {

            _service.Delete(id.Value);

            TempData["ShowSuccessAlert"] = true;
            TempData["SweetAlertTitle"] = "該購物車物品已刪除";
            TempData["SweetAlertText"] = "該購物車物品已成功刪除。";
            TempData["SweetAlertIcon"] = "success";

            return RedirectToAction("Index", "Carts");
        }
    }
}