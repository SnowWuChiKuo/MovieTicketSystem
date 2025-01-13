using ClientSide.Models.EFModels;
using ClientSide.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ClientSide.Models.DAOs
{
    public class CartItemEFDao
    {
        private readonly AppDbContext _db = new AppDbContext();

        /// <summary>
        /// 清除存在超過五分鐘的 CartItem 資料。
        /// </summary>
        /// <returns></returns>
        public async Task ClearExpiredCartItems()
        {
            var threshold = DateTime.Now.AddMinutes(-5);
            var now = DateTime.Now;

            // 使用 Entity Framework Core 的 RemoveRange 方法，刪除過期的購物車項目
            var expiredCartItems = _db.CartItems
                 .Include(ci => ci.Ticket.Screening) // 加入 Ticket 和 Screening
                                                     //.Where(ci => ci.CreatedAt < threshold ||
                                                     //     DbFunctions.DiffMinutes(now, DbFunctions.CreateDateTime(ci.Ticket.Screening.Televising.Year,
                                                     //     ci.Ticket.Screening.Televising.Month,
                                                     //     ci.Ticket.Screening.Televising.Day,
                                                     //     ci.Ticket.Screening.StartTime.Hours,
                                                     //     ci.Ticket.Screening.StartTime.Minutes,
                                                     //     0)) < 30
                                                     // );
            .Where(ci => ci.CreatedAt < threshold);

            _db.CartItems.RemoveRange(expiredCartItems);
            await _db.SaveChangesAsync();
            _db.Set<CartItem>().Local.Clear();//使用 DbSet<T>.Local.Clear() 方法
        }

        public CartItemDetailVm Get(int id)
        {
            CartItem cartitem = _db.CartItems.FirstOrDefault(ci => ci.Id == id);

            CartItemDetailVm vm = new CartItemDetailVm
            {
                Id = id,
                TheaterName = GetTheaterName(id),
                MovieTitle = GetMovieTitle(id),
                MovieTime = GetScreeningTime(id),
                SeatName = GetSeatName(id),
                Qty = cartitem.Qty,
                Price = GetPrice(id),
                SubTotal = cartitem.SubTotal
            };
            return vm;
                
        }

        /// <summary>
        /// 取得影廳名稱
        /// </summary>
        /// <param name="cartItemId"></param>
        /// <returns></returns>
        public string GetTheaterName(int cartItemId)
        {
            var cartItem = _db.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
            var ticket  = _db.Tickets.FirstOrDefault(t=>t.Id == cartItem.TicketId);  
            var screening = _db.Screenings.FirstOrDefault(s => s.Id == ticket.ScreeningId);
            var theater = _db.Theaters.FirstOrDefault(th => th.Id == screening.Id);

            return theater.Name;
        }

        /// <summary>
        /// 取得電影名稱
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        public string GetMovieTitle(int cartItemId)
        {
            var cartItem = _db.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
            var ticket = _db.Tickets.FirstOrDefault(t => t.Id == cartItem.TicketId);
            var screening = _db.Screenings.FirstOrDefault(s => s.Id == ticket.ScreeningId);
            var movie = _db.Movies.FirstOrDefault(m => m.Id == screening.MovieId);

            return movie.Title;
        }

        /// <summary>
        /// 取得放映時間 時間 StartTime ~ EndTime
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        public string GetScreeningTime(int cartItemId)
        {
            var cartItem = _db.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
            var ticket = _db.Tickets.FirstOrDefault(t => t.Id == cartItem.TicketId);
            var screening = _db.Screenings.FirstOrDefault(s => s.Id == ticket.ScreeningId);

            return $"{screening.Televising.ToString("yyyy-MM-dd")} {screening.StartTime} - {screening.EndTime}";
        }

        /// <summary>
        /// 取得座位名稱
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        public string GetSeatName(int cartItemId) //row number
        {
            var cartItem = _db.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
            var ticket = _db.Tickets.FirstOrDefault(t => t.Id == cartItem.TicketId);
            var screening = _db.Screenings.FirstOrDefault(s => s.Id == ticket.ScreeningId);
            var seatStatus = _db.SeatStatus.FirstOrDefault(ss => ss.ScreeningId == screening.Id);
            var seat = _db.Seats.FirstOrDefault(s=>s.Id == seatStatus.SeatId);

            return $"{seat.Row}{seat.Number}";
        }

        /// <summary>
        /// 拿到單票價格
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        public int GetPrice(int cartItemId)
        {
            var cartItem = _db.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
            var ticket = _db.Tickets.FirstOrDefault(t => t.Id == cartItem.TicketId);
            return ticket.Price;
        }
    }
}