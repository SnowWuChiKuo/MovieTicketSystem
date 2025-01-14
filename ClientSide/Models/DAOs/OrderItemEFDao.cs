using ClientSide.Models.EFModels;
using ClientSide.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientSide.Models.DAOs
{
    public class OrderItemEFDao
    {
        private readonly AppDbContext _db = new AppDbContext();

        public OrderItemDetailVm Get(int id)
        {
            OrderItem orderitem = _db.OrderItems.FirstOrDefault(oi => oi.Id == id);

            OrderItemDetailVm vm = new OrderItemDetailVm
            {
                Id = id,
                TheaterName = GetTheaterName(id),
                MovieTitle = GetMovieTitle(id),
                MovieTime = GetScreeningTime(id),
                SeatNames = GetSeatNames(orderitem.SeatNames),
                Qty = orderitem.Qty,
                Price = GetPrice(id),
                SubTotal = orderitem.SubTotal,
            };

            return vm;
        }

       

        // <summary>
        /// 取得影廳名稱
        /// </summary>
        /// <param name="cartItemId"></param>
        /// <returns></returns>
        public string GetTheaterName(int orderItemId)
        {
            var orderItem = _db.OrderItems.FirstOrDefault(ci => ci.Id == orderItemId);
            var ticket = _db.Tickets.FirstOrDefault(t => t.Id == orderItem.TicketId);
            var screening = _db.Screenings.FirstOrDefault(s => s.Id == ticket.ScreeningId);
            var theater = _db.Theaters.FirstOrDefault(th => th.Id == screening.TheaterId);

            return theater.Name;
        }

        /// <summary>
        /// 取得電影名稱
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        public string GetMovieTitle(int orderItemId)
        {
            var orderItem = _db.OrderItems.FirstOrDefault(ci => ci.Id == orderItemId);
            var ticket = _db.Tickets.FirstOrDefault(t => t.Id == orderItem.TicketId);
            var screening = _db.Screenings.FirstOrDefault(s => s.Id == ticket.ScreeningId);
            var movie = _db.Movies.FirstOrDefault(m => m.Id == screening.MovieId);

            return movie.Title;
        }

        /// <summary>
        /// 取得放映時間 時間 StartTime ~ EndTime
        /// </summary>
        /// <param name="ticketId"></param>
        /// <returns></returns>
        public string GetScreeningTime(int orderItemId)
        {
            var orderItem = _db.OrderItems.FirstOrDefault(ci => ci.Id == orderItemId);
            var ticket = _db.Tickets.FirstOrDefault(t => t.Id == orderItem.TicketId);
            var screening = _db.Screenings.FirstOrDefault(s => s.Id == ticket.ScreeningId);

            return $"{screening.Televising.ToString("yyyy-MM-dd")} {screening.StartTime} - {screening.EndTime}";
        }

		/// <summary>
		/// 取得座位名稱，若有多個座位就全部傳回，這個值在Ticket/Index就要傳
		/// </summary>
		/// <param name="cartItemId"></param>
		/// <returns></returns>
		public string GetSeatNames(string seatName) //row number
		{
			return $"{seatName.Trim()}";
		}

		/// <summary>
		/// 拿到單票價格
		/// </summary>
		/// <param name="ticketId"></param>
		/// <returns></returns>
		public int GetPrice(int orderItemId)
        {
            var orderItem = _db.OrderItems.FirstOrDefault(ci => ci.Id == orderItemId);
            var ticket = _db.Tickets.FirstOrDefault(t => t.Id == orderItem.TicketId);
            return ticket.Price;
        }
    }
}