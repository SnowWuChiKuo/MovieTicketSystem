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
    public class CartsController : Controller
    {
        private readonly CartService _service = new CartService();

        // GET: Cart
        [Authorize]
        public ActionResult Index()
        {
            string account = User.Identity.Name;
            CartVm cart =  GetCartInfo(account);

            return View(cart);
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
        public ActionResult Checkout(int? cartId)
        {
            if (!cartId.HasValue)
            {
                return Content("購物車 ID 無效，無法結帳!");
            }
            string account = User.Identity.Name;
            CartVm cart = GetCartInfo(account);

            if (cart.AllowCheckout == false)
            {
                return Content("購物車內沒有商品，無法結帳!");
            }

            List<int> cartItemIds = _service.GetCartItemIds(cartId.Value);
            bool isValid = _service.CheckIfCartItemsValid(cartItemIds);
            //驗證通過能結帳
            if (isValid)
            {
                //建立訂單主檔/明細檔
                _service.CreateOrder(account);

                //清空購物車
                _service.EmptyCart(account);

                return Redirect("~/Orders/Index");  // 結帳成功畫面
            }
            else 
            {
                TempData["ErrorMessage"] = "訂票夾中存在無效票，無法結帳，請移除後再試";

                return RedirectToAction("Index");
            }
        }

    }
}