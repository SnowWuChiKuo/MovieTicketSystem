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
        //傳入商品id與數量
        public ActionResult AddItem(int ticketId ,int qty)
        {
            string account = User.Identity.Name; //豋入者的 account
            //int qty = 1; 

            Add2Cart(account, ticketId, qty);

            return new EmptyResult();
        }
        private void Add2Cart(string account, int productId, int qty)
        {
            //取得目前購物車，若沒購物車則新增一筆
            CartVm cart = _service.GetCartInfo(account);

            //取得購物車Id
            int cartId = cart.Id;

            //將商品加入購物車
            _service.AddCartItem(cartId, productId, qty);
        }

        private CartVm GetCartInfo(string account)
        {
            return _service.GetCartInfo(account);
        }

        [Authorize]
        public ActionResult Checkout()
        {
            string account = User.Identity.Name;
            CartVm cart = GetCartInfo(account);

            if(cart.AllowCheckout == false)
            {
                return Content("購物車內沒有商品，無法結帳!");
            }

            return View();
        }


        /// <summary>
        /// 結帳
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Checkout(CheckoutVm model)
        {
            string account = User.Identity.Name;

            //建立訂單主檔/明細檔
            _service.CreateOrder(account, model);

            //清空購物車
            _service.EmptyCart(account);

            return View("ConfirmCheckout");  // 結帳成功畫面
        }

        


    }
}