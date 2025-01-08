using Microsoft.EntityFrameworkCore;
using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.DAOs
{
    public class OrderItemDao
    {
        private readonly AppDbContext _db;

        public OrderItemDao(AppDbContext db)
        {
            _db = db;
        }

        public List<OrderItemVm> GetAll()
        {
            var data = _db.OrderItems.Include(d => d.Order)
                                      .Include(d => d.Ticket)
                                      .Select(d => new OrderItemVm
                                      {
                                          Id = d.Id,
                                          OrderId = d.OrderId,
                                          TicketId = d.TicketId,
                                          TicketName = d.TicketName,
                                          Price = d.Price,
                                          Qty = d.Qty,
                                          SubTotal = d.SubTotal,
                                      }).ToList();
            return data;
        }

        public Ticket GetTicketNameById(int ticketId)
        {
            // 根據 ticketId 從資料庫中獲取 TicketName
            var ticket = _db.Tickets.FirstOrDefault(t => t.Id == ticketId);

            if (ticket == null) throw new Exception("找不到此票種!");

            return ticket;
        }

        public Dictionary<int, int> GetAllOrderQty()
        {
            var orderQty = _db.OrderItems
                              .GroupBy(o => o.OrderId)
                              .Select(g => new
                              {
                                  OrderId = g.Key,
                                  TotalQty = g.Sum(o => o.Qty)
                              })
                              .ToDictionary(x => x.OrderId, x => x.TotalQty);

            return orderQty;
        }

        public void Create(OrderItemDto dto)
        {
            OrderItem orderItem = ConvertTOEntity(dto);
            _db.OrderItems.Add(orderItem);
            _db.SaveChanges();
        }

        private OrderItem ConvertTOEntity(OrderItemDto dto)
        {
            return new OrderItem
            {
                Id= dto.Id,
                OrderId= dto.OrderId,
                TicketId= dto.TicketId,
                TicketName= dto.TicketName,
                Price= dto.Price,
                Qty= dto.Qty,
                SubTotal= dto.SubTotal,
            };
        }

        public OrderItemVm Get(int id)
        {
            var orderItem = _db.OrderItems.FirstOrDefault(o => o.Id == id);

            if (orderItem == null)
            {
                throw new Exception("找不到此電影票!");
            }

            return new OrderItemVm
            {
                Id = orderItem.Id,
                OrderId= orderItem.OrderId,
                TicketId= orderItem.TicketId,
                TicketName= orderItem.TicketName,
                Price= orderItem.Price,
                Qty= orderItem.Qty,
                SubTotal= orderItem.SubTotal,
            };
        }

        public OrderItem GetOrderItemById(int id)
        {
            var data = _db.OrderItems.FirstOrDefault(d => d.Id == id);

            if (data == null) throw new Exception("找不到此電影票!");

            return data;
        }

        public void Edit(OrderItem orderItem)
        {
            _db.OrderItems.Update(orderItem);
            _db.SaveChanges();
        }

        public void Delete(OrderItem orderItem)
        {
            _db.OrderItems.Remove(orderItem);
            _db.SaveChanges();
        }
    }
}
