using ClientSide.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ClientSide.Models.ViewModels;

namespace ClientSide.Models.DAOs
{
    public class OrderEFDao
    {
        private readonly AppDbContext _db = new AppDbContext();

		public List<OrderVm> GetOrderInfo(string account)
		{
			// 一次性查询所需的所有相关数据，避免在循环中查询
			var orders = (from o in _db.Orders
						  join m in _db.Members on o.MemberId equals m.Id
						  where m.Account == account
						  select o).ToList();

			if (!orders.Any()) throw new Exception("找不到任何訂單!");

			var orderIds = orders.Select(o => o.Id).ToList();

			// 先获取原始数据
			var orderItemsData = (from oi in _db.OrderItems
								  join t in _db.Tickets on oi.TicketId equals t.Id
								  join s in _db.Screenings on t.ScreeningId equals s.Id
								  join mov in _db.Movies on s.MovieId equals mov.Id
								  where orderIds.Contains(oi.OrderId)
								  select new
								  {
									  OrderItem = oi,
									  Screening = s,
									  Movie = mov
								  }).ToList();  // 先将数据取出来

			// 在内存中进行数据转换
			var orderItems = orderItemsData.Select(x => new OrderItemVm
			{
				Id = x.OrderItem.Id,
				OrderId = x.OrderItem.OrderId,
				TicketId = x.OrderItem.TicketId,
				TicketName = x.OrderItem.TicketName,
				Price = x.OrderItem.Price,
				Qty = x.OrderItem.Qty,
				SubTotal = x.OrderItem.SubTotal,
				SeatNames = x.OrderItem.SeatNames,
				MovieTime = $"{x.Screening.Televising:yyyy-MM-dd} {x.Screening.StartTime} - {x.Screening.EndTime}",
				MovieTitle = x.Movie.Title,
				ImgPath = x.Movie.PosterURL
			}).ToList();

			// 组装最终结果
			var orderList = orders.Select(order => new OrderVm
			{
				Id = order.Id,
				OrderItems = orderItems.Where(oi => oi.OrderId == order.Id).ToList()
			}).ToList();

			return orderList;
		}


		private string GetScreeningTime(int ticketId)
        {
            var ticket = _db.Tickets.FirstOrDefault(t => t.Id == ticketId);
            var screening = _db.Screenings.FirstOrDefault(s => s.Id == ticket.ScreeningId);

            return $"{screening.Televising.ToString("yyyy-MM-dd")} {screening.StartTime} - {screening.EndTime}";
        }

        private string GetMovieTitle(int ticketId)
        {
            var ticket = _db.Tickets.FirstOrDefault(t => t.Id == ticketId);
            var screening = _db.Screenings.FirstOrDefault(s => s.Id == ticket.ScreeningId);
            var movie = _db.Movies.FirstOrDefault(m => m.Id == screening.MovieId);

            return movie.Title;
        }

        private string GetImageName(int ticketId)
        {
            var ticket = _db.Tickets.FirstOrDefault(t => t.Id == ticketId);
            var screening = _db.Screenings.FirstOrDefault(s => s.Id == ticket.ScreeningId);
            var movie = _db.Movies.FirstOrDefault(m => m.Id == screening.MovieId);

            return movie.PosterURL;
        }
    }
}