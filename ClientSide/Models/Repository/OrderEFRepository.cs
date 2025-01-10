using ClientSide.Models.EFModels;
using ClientSide.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientSide.Models.Repository
{
    public class OrderEFRepository
    {
        private readonly AppDbContext _db;

        public OrderEFRepository(AppDbContext db)
        {
            _db = db;
        }

        public void CreateOrder(string account, CheckoutVm model)
        {
            var member = _db.Members.FirstOrDefault(m => m.Account == account);

            if (member == null) throw new Exception("找不到此會員!");

            var cart = _db.Carts.FirstOrDefault(c => c.MemberId == member.Id);

            if (cart == null) throw new Exception("找不到此購物車!");


            // 新增訂單主檔
            var order = new Order
            {
                MemberId = _db.Members.FirstOrDefault(m => m.Account == account).Id,
                CouponId = _db.Coupons.FirstOrDefault(c => c.Code == model.CouponCode).Id,
                TotalAmount = model.TotalAmount,
                Status = model.Status,
                DiscountPrice = model.DiscountPrice,
            };

            // 新增訂單明細
            foreach (var item in cart.CartItems)
            {
                var orderItem = new OrderItem
                {
                    //用此法，可以與訂單主檔建立關聯，不必知道 OrderId
                    Order = order,
                    TicketId = item.TicketId,
                    TicketName = $"{item.Ticket.TicketType} - {item.Ticket.SalesType}",
                    Price = item.Ticket.Price,
                    Qty = item.Qty,
                    SubTotal = item.SubTotal
                };

                _db.OrderItems.Add(orderItem);
            }

            // 加入訂單主檔
            _db.Orders.Add(order);
            // 會一次將訂單主檔與明細一起存檔，且包在一個 transaction 內
            _db.SaveChanges();

        }

        public void EmptyCart(string account)
        {
            var member = _db.Members.FirstOrDefault(m => m.Account == account);

            if (member == null) throw new Exception("找不到此會員!");

            var cart = _db.Carts.FirstOrDefault(c => c.MemberId == member.Id);

            if (cart == null) throw new Exception("找不到此購物車!");

            _db.Carts.Remove(cart);
            _db.SaveChanges(); // 資料庫設定"重疊顯示" => cartItem 所以會一起刪除!
        }
    }
}