using ClientSide.Models.DAOs;
using ClientSide.Models.EFModels;
using ClientSide.Models.Repository;
using ClientSide.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientSide.Models.Services
{
    public class CartService
    {
        private readonly CartEFDao _cartDao;

        private readonly OrderEFRepository _orderRp;
        public CartService(CartEFDao dao, OrderEFRepository orderRp)
        {
            _cartDao = dao;
            _orderRp = orderRp;
        }

        public CartVm GetCartInfo(string account)
        {
            var cart = _cartDao.GetCart(account);

            if (cart == null)
            {
                cart = _cartDao.CreateCart(account);
            }

            var cartItems = _cartDao.AddCartItems(cart);

            // 建立空的 CartVm 物件，並把找到的資料帶進去
            var cartVm = new CartVm
            {
                Id = cart.Id,
                MemberId = cart.MemberId,
                CartItems = cartItems,
            };
            return cartVm;
        }

        public void CreateOrder(string account, CheckoutVm model)
        {
            var cart = GetCartInfo(account);

            _orderRp.CreateOrder(account, model);
        }

        public void EmptyCart(string account)
        {
            _orderRp.EmptyCart(account);
        }
    }
}