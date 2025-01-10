using ClientSide.Models.Services;
using ClientSide.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace ClientSide.Controllers
{   
    public class CartController : Controller
    {
        private readonly CartService _service;

        public CartController(CartService service)
        {
            _service = service;
        }

        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Checkout()
        {
            string account = User.Identity.Name;
            var cart = GetCartInfo(account);

            if(cart.AllowCheckout == false)
            {
                return Content("購物車內沒有商品，無法結帳!");
            }

            return View();
        }

        private CartVm GetCartInfo(string account)
        {
            return _service.GetCartInfo(account);
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout(CheckoutVm model)
        {
            string account = User.Identity.Name;
            var cart = GetCartInfo(account);

            if (cart.AllowCheckout == false)
            {
                return Content("購物車內沒有商品，無法結帳!");
            }

            _service.CreateOrder(account, model);

            _service.EmptyCart(account);

            return View("ConfirmCheckout");  // 結帳成功畫面
        }


    }
}