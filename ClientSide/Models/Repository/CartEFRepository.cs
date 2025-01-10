﻿using ClientSide.Models.EFModels;
using System.Data.Entity; // 使 Include 能用
using ClientSide.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;

namespace ClientSide.Models.Repository
{
    public class CartEFRepository
    {
        private readonly AppDbContext _db;

        public CartEFRepository(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// 創建構物車
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Cart CreateCart(string account)
        {
            var member = _db.Members.FirstOrDefault(m => m.Account == account);

            if (member == null) throw new Exception("找不到此會員!");

            var cart = new Cart
            {
                MemberId = member.Id,
                CreatedAt = DateTime.Now
            };

            _db.Carts.Add(cart);
            _db.SaveChanges();

            return cart;
        }

        /// <summary>
		/// 取得目前的購物車主檔，若沒有，就新增一筆並傳回
		/// </summary>
		/// <param name="account"></param>
		/// <returns></returns>
		public CartVm GetCartInfo(string account)
        {

            Cart cart = _db.Carts.Include(c => c.Member).FirstOrDefault(c => c.Member.Account == account);

            if (cart == null) //沒有購物車就新增一筆
            {
                cart = CreateCart(account);
            }

            //目前已經加入過的商品
            List<CartItemVm> cartItems = _db.CartItems.Where(ci => ci.CartId == cart.Id).OrderBy(ci => ci.CreatedAt)
                .Select(ci => new CartItemVm
                {
                    Id = ci.Id,
                    CartId = ci.CartId,
                    TicketId = ci.TicketId,
                    Qty = ci.Qty,
                    SubTotal = ci.SubTotal,
                    ImgPath = GetImageName(ci.TicketId),
                    MovieTitle = GetMovieTitle(ci.TicketId),
                    MovieTime = GetScreeningTime(ci.TicketId)
                }).ToList();

            //建立購物車物件
            var cartVm = new CartVm
            {
                Id = cart.Id,
                CartItems = cartItems
            };

            //傳回物件
            return cartVm;

        }

        /// <summary>
        /// 取得放映時間 時間 StartTime ~ EndTime
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        public string GetScreeningTime(int ticketId)
        {
            var ticket = _db.Tickets.FirstOrDefault(t => t.Id == ticketId);
            var screening = _db.Screenings.FirstOrDefault(s => s.Id == ticket.ScreeningId);

            return $"{screening.Televising} {screening.StartTime} - {screening.EndTime}";
        }

        /// <summary>
        /// 取得電影海報名稱
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        public string GetImageName(int ticketId)
        {
            var ticket = _db.Tickets.FirstOrDefault(t => t.Id == ticketId);
            var screening = _db.Screenings.FirstOrDefault(s => s.Id == ticket.ScreeningId);
            var movie = _db.Movies.FirstOrDefault(m => m.Id == screening.MovieId);

            return movie.PosterURL;
        }

        /// <summary>
        /// 取得電影名稱
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        public string GetMovieTitle(int ticketId)
        {
            var ticket = _db.Tickets.FirstOrDefault(t => t.Id == ticketId);
            var screening = _db.Screenings.FirstOrDefault(s => s.Id == ticket.ScreeningId);
            var movie = _db.Movies.FirstOrDefault(m => m.Id == screening.MovieId);

            return movie.Title;
        }

        /// <summary>
        /// 加入購物車，若明細不存在，就新增一筆，若存在就更新數量
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="ticketId"></param>
        /// <param name="qty"></param>
        public void AddCartItem(int cartId, int ticketId, int qty)
        {
            var cartItemInDb = _db.CartItems.FirstOrDefault(ci => ci.CartId == cartId && ci.TicketId == ticketId);
            //已存在相同項目
            if (cartItemInDb != null)
            {
                cartItemInDb.Qty += qty;
                _db.SaveChanges();
            }
            else //不存在相同項目
            {
                CartItem cartItem = new CartItem
                {
                    CartId = cartId,
                    TicketId = ticketId,
                    Qty = qty,
                    SubTotal = GetSubTotal(ticketId, qty),
                    CreatedAt = DateTime.Now
                };
                _db.CartItems.Add(cartItem);
                _db.SaveChanges();
            }

        }

        /// <summary>
        /// 拿到項目小計
        /// </summary>
        /// <param name="ticketId"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        public int GetSubTotal(int ticketId, int qty)
        {
            var ticket = _db.Tickets.FirstOrDefault(t => t.Id == ticketId);
            return ticket.Price * qty;
        }

        /// <summary>
        /// 拿到單票價格
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        public int GetPrice(int ticketId)
        {
            var ticket = _db.Tickets.FirstOrDefault(t => t.Id == ticketId);
            return ticket.Price ;
        }

        /// <summary>
        /// 清空購物車，刪除 Cart 物件與其內 CartItems
        /// </summary>
        /// <param name="account"></param>
        public void EmptyCart(string account)
        {
            var cart = _db.Carts.Include(c=>c.Member).FirstOrDefault(c => c.Member.Account == account);

            if (cart == null) return;

            _db.Carts.Remove(cart);

            _db.SaveChanges();
        }

        /// <summary>
        /// 建立訂單主檔 / 明細檔
        /// </summary>
        /// <param name="account"></param>
        /// <param name="model"></param>
        public void CreateOrder(string account, CheckoutVm model)
        {
            
                CartVm cart = GetCartInfo(account);
                //新增訂單主檔
                var order = new Order
                {
                    MemberId = _db.Members.FirstOrDefault(m => m.Account == account).Id,
                    CouponId = _db.Coupons.FirstOrDefault(co=>co.Code == model.CouponCode).Id,
                    TotalAmount = cart.Total,
                    CreatedAt = DateTime.Now,
                    Status = true ,//true : 預設已付款
                    DiscountPrice = model.DiscountPrice
                };//這裡不必叫用SaveChanges

                //新增訂單明細檔
                foreach (var item in cart.CartItems)
                {
                    var orderItem = new OrderItem
                    {
                        Order = order, //用此法，可以與訂單主檔建立關聯，不必知道orderId
                        TicketId = item.TicketId,
                        TicketName = item.MovieTitle,
                        Qty = item.Qty,
                        Price = GetPrice(item.TicketId),
                        SubTotal = item.SubTotal,
                    };
                   _db.OrderItems.Add(orderItem);
                }

                _db.Orders.Add(order);
                _db.SaveChanges(); //會一次將訂單主檔/明細檔一起存檔，且包在一個transaction內

        }

        //public int CalculateDiscountPrice(CheckoutVm model)
        //{
        //    var coupon = _db.Coupons.FirstOrDefault(c => c.Code == model.CouponCode);
        //    if (coupon == null)
        //    {
        //        throw new Exception("無此優惠卷");
        //    }
        //    else
        //    {
        //        if(coupon.DiscountType == "元")
        //        {

        //        }
        //        else //coupon.DiscountType == "%"
        //        {

        //        }
        //    }
        //}
    }
}