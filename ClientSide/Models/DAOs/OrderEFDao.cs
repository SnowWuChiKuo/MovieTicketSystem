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

        public OrderVm GetOrderInfo(string account)
        {
            Order order = _db.Orders.Include(d => d.Member).FirstOrDefault(d => d.Member.Account == account);

            List<OrderItemVm> orderItems = _db.OrderItems
                .Where(oi => oi.OrderId == order.Id)
                .OrderBy(oi => oi.Order.CreatedAt)
                .ToList()
                .Select(oi => new OrderItemVm
                {
                    Id = oi.Id,
                    OrderId = oi.OrderId,
                    TicketId = oi.TicketId,
                    TicketName = oi.TicketName,
                    Price = oi.Price,
                    Qty = oi.Qty,
                    SubTotal = oi.SubTotal,
                    ImgPath = GetImageName(oi.TicketId),
                    MovieTitle = GetMovieTitle(oi.TicketId),
                    MovieTime = GetScreeningTime(oi.TicketId),
                }).ToList();

            var orderVm = new OrderVm
            {
                Id = order.Id,
                OrderItems = orderItems,
            };

            return orderVm;
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