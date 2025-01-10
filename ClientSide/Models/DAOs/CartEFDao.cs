using ClientSide.Models.EFModels;
using ClientSide.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace ClientSide.Models.DAOs
{
    public class CartEFDao
    {
        private readonly AppDbContext _db;

        public CartEFDao(AppDbContext db)
        {
            _db = db;
        }

        public Cart CreateCart(string account)
        {
            var member = _db.Members.FirstOrDefault(m => m.Account == account);

            if (member == null) throw new Exception("找不到此會員!");

            var cart = new Cart 
            {
                MemberId = member.Id
            };

            _db.Carts.Add(cart);
            _db.SaveChanges();

            return cart;
        }

        public List<CartItemVm> AddCartItems(Cart cart)
        {
            var cartItems = _db.CartItems.AsNoTracking()
                                         .Include(ci => ci.Ticket)
                                         .Include(ci => ci.Cart)
                                         .Where(ci => ci.CartId == cart.Id)
                                         .Select(ci => new CartItemVm
                                         {
                                             Id = ci.Id,
                                             CartId = ci.CartId,
                                             TicketId = ci.TicketId,
                                             Qty = ci.Qty,
                                             SubTotal = ci.SubTotal,
                                         }).ToList();
            return cartItems;
        }


        public Cart GetCart(string account)
        {
            var cart = _db.Carts.Include(c => c.Member)
                                .FirstOrDefault(c => c.Member.Account == account);

            return cart;
        }



    }
}