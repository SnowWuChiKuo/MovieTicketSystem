using ClientSide.Models.EFModels;
using ClientSide.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientSide.Models.DAOs
{
    public class CartItemEFDao
    {
        private readonly AppDbContext _db = new AppDbContext();
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