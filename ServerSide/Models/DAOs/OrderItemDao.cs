using Microsoft.EntityFrameworkCore;
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

            // ?? 是空合併運算符，它的作用是如果左邊的表達式為 null，則返回右邊的值。
            return ticket;
        }

    }
}
