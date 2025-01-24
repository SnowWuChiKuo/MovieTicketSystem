using ClientSide.Models.EFModels;
using System.Data.Entity; // 使 Include 能用
using ClientSide.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using System.Data;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Windows.Documents;


namespace ClientSide.Models.Repository
{
    public class CartEFRepository
    {
        private readonly AppDbContext _db = new AppDbContext();

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
            List<CartItemVm> cartItems = _db.CartItems
                .Where(ci => ci.CartId == cart.Id)
                .OrderBy(ci => ci.CreatedAt)
                .ToList() // 先把資料抓出來 List<CartItem>
                .Select(ci => new CartItemVm
                {
                    Id = ci.Id,
                    CartId = ci.CartId,
                    TicketId = ci.TicketId,
                    Qty = ci.Qty,
                    SubTotal = ci.SubTotal,
                    SeatName = ci.SeatsName,
                    ImgPath = GetImageName(ci.TicketId),
                    MovieTitle = GetMovieTitle(ci.TicketId),
                    MovieTime = GetScreeningTime(ci.TicketId)
                }).ToList(); // List<CartItemVm>
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

            return $"{screening.Televising.ToString("yyyy-MM-dd")} {screening.StartTime} - {screening.EndTime}";
        }
        /// <summary>
        /// 取得所有購物車物品放映開始時間 StartTime 
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        public List<DateTime> GetScreeningStartTime(List<int> cartItemIds)
        {
            List<DateTime> result = new List<DateTime>();
            foreach (var id in cartItemIds)
            {
                var cartItem = _db.CartItems.FirstOrDefault(ci => ci.Id == id);
                var ticket = _db.Tickets.FirstOrDefault(t => t.Id == cartItem.TicketId);
                var screening = _db.Screenings.FirstOrDefault(s => s.Id == ticket.ScreeningId);

                var startTime  =  screening.Televising.Date + screening.StartTime;
                result.Add(startTime);
            }

            return result;
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
        /// 加入購物車，若明細不存在，就新增一筆，若存在就更新數量，選票頁面要傳入seatName
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="ticketId"></param>
        /// <param name="qty"></param>
        /// <param name="seatName"></param>
        public void AddCartItem(int cartId, int ticketId, int qty , string seatName)
        {
            seatName = RemoveSpaces(seatName);
            //尋找是否存在相同項目
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
                    CreatedAt = DateTime.Now,
                    SeatsName =  seatName
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
        /// 購物車裡面的全部票卷數量
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
		public int CartItemTotalQty(int cartId)
        {
            var cartItem = _db.CartItems.Where(ci => ci.CartId == cartId);

            int currentQty = 0;

            foreach (var item in cartItem)
            {
                currentQty += item.Qty;
            }

            return currentQty;
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
        /// 需要找到場次後，確認位子狀態
        /// </summary>
        /// <param name="seatName"></param>
        /// <param name="screeningId"></param>
        public List<SeatStatus> CheckSeatStatus(string seatName, int screeningId)
        {
            var screening = _db.Screenings.FirstOrDefault(s => s.Id == screeningId);

            string[] seats = seatName.Split('、');

			var SeatStatusList = new List<SeatStatus>();

			foreach (var item in seats)
			{
				// 使用 "排" 分割取得行號和座位號
				string[] parts = item.Split('排');
				string[] parts2 = parts[1].Split('號');

				string row = parts[0];                    // 排之前的字母
				string number = parts2[0];

				var seat = _db.Seats.FirstOrDefault(s => s.Row == row && s.Number == number && s.TheaterId == screening.TheaterId);

				var SeatStatus = _db.SeatStatus.FirstOrDefault(ss => ss.SeatId == seat.Id && ss.ScreeningId == screeningId);

				SeatStatusList.Add(SeatStatus);

			}

            return SeatStatusList;
		}

		/// <summary>
		/// 建立訂單主檔 / 明細檔
		/// </summary>
		/// <param name="account"></param>
		/// <param name="model"></param>
		public void CreateOrder(string account, List<SeatStatus> seatstatus)
        {
            
                CartVm cart = GetCartInfo(account);
                //新增訂單主檔
                var order = new Order
                {
                    MemberId = _db.Members.FirstOrDefault(m => m.Account == account).Id,
                    //CouponId = _db.Coupons.FirstOrDefault(co=>co.Code == model.CouponCode).Id,
                    TotalAmount = cart.Total,
                    CreatedAt = DateTime.Now,
                    Status = true ,//true : 預設已付款
                    //DiscountPrice = model.DiscountPrice
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
                        SeatNames = item.SeatName
                    };

                    UpdateSeatStatus(seatstatus);
                    
                   _db.OrderItems.Add(orderItem);
                }

                _db.Orders.Add(order);
                _db.SaveChanges(); //會一次將訂單主檔/明細檔一起存檔，且包在一個transaction內


                
        }

        private void UpdateSeatStatus(List<SeatStatus> seatstatus)
        {
			var SeatStatusList = new List<SeatStatus>();

			foreach (var item in seatstatus)
            {
                if (item.Status == "不可使用") throw new Exception($"{item.Seat.Row} {item.Seat.Number}此座位已經被訂走了!");

				item.Status = "不可使用";
				item.UpdatedAt = DateTime.Now;

                SeatStatusList.Add(item);
            }

            _db.SeatStatus.AddRange(SeatStatusList);
        }

        /// <summary>
        /// 拿到  陣列
        /// </summary>
        /// <param name="cartItemIds"></param>
        /// <param name="seatName"></param>
        /// <param name="screeningId"></param>
        /// <returns></returns>
        //public List<string> GetSeatStatus(List<int> cartItemIds , string seatName ,int screeningId)
        //{

        //    List<string> result = new List<string>();
        //    foreach (var id in cartItemIds)
        //    {
        //        var cartItem = _db.CartItems.FirstOrDefault(ci => ci.Id == id);
        //        var ticket = _db.Tickets.FirstOrDefault(t => t.Id == cartItem.TicketId);
        //        var screening = _db.Screenings.FirstOrDefault(s => s.Id == ticket.ScreeningId);
        //        var SeatStatus = _db.SeatStatus.FirstOrDefault(ss => ss.ScreeningId == screening.Id);

        //        result.Add(SeatStatus.Status);
        //    }

        //    return result;
        //}

        public List<int> GetCartItemIds(int cartId)
        {
            List<CartItem> cartItems = _db.CartItems.Where(ci => ci.CartId == cartId).ToList();
            List<int> cartItemIds = new List<int>();
            foreach (var item in cartItems)
            {
                cartItemIds.Add(item.Id);
            }

            return cartItemIds;
        }
        public string RemoveSpaces( string input)
        {
            return input.Replace(" ", string.Empty);
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