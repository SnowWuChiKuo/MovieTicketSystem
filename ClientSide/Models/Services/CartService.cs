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
        private readonly CartEFRepository _repo;

        //private readonly OrderEFRepository _orderRp;
        public CartService(CartEFRepository repo)
        {
            _repo = repo;
        }

        public CartVm GetCartInfo(string account)
        {
            CartVm cart = _repo.GetCartInfo(account);

            return cart;
        }

        public void CreateOrder(string account, CheckoutVm model)
        {
            _repo.CreateOrder(account, model);
        }

        public void EmptyCart(string account)
        {
            _repo.EmptyCart(account);
        }

        public void AddCartItem(int cartId, int productId, int qty)
        {
            _repo.AddCartItem(cartId, productId, qty);
        }

        //public int CalculateDiscountPrice(CheckoutVm model)
        //{
        //    return _repo.CalculateDiscountPrice(model);
        //}
    }
}