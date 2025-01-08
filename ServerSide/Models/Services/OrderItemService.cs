using Humanizer;
using ServerSide.Models.DAOs;
using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.Services
{
    public class OrderItemService
    {
        private readonly OrderItemDao _dao;

        public OrderItemService(OrderItemDao dao)
        {
            _dao = dao;
        }

        public List<OrderItemVm> GetAll()
        {
            return _dao.GetAll();
        }

        public Ticket GetTicketNameById(int ticketId)
        {
            // 根據 ticketId 從資料存取層獲取 TicketName
            return _dao.GetTicketNameById(ticketId);
        }

        public void Create(OrderItemDto dto)
        {
            if (dto.Qty >= 6) throw new Exception("總數量不可超過6張");

            // 找出這張訂單有多少數量的東西
            var orderQty = _dao.GetAllOrderQty();

            // 確認是否有相同的 OrderId
            if (orderQty.TryGetValue(dto.OrderId, out int existingQty))
            {
                var totalQty = dto.Qty + existingQty;

                if (totalQty > 6)
                {
                    throw new Exception("總數量不可超過6張");
                }
            }


            _dao.Create(dto);
        }

        public OrderItemVm Get(int id)
        {
            return _dao.Get(id);
        }

        public void Edit(OrderItemDto dto)
        {
            var orderItemInDb = _dao.GetOrderItemById(dto.Id);

            if (orderItemInDb == null) throw new Exception("找不到此電影票!");

            if (dto.Qty > 6) throw new Exception("總數量不可超過6張");

            // 找出這張訂單有多少數量的東西
            var orderQty = _dao.GetAllOrderQty();

            // 確認是否有相同的 OrderId
            if (orderQty.TryGetValue(dto.OrderId, out int existingQty))
            {
                var totalQty = dto.Qty + existingQty;

                // orderItemInDb 是資料庫值  dto是輸入值   existingQty是訂單所有數量
                if (existingQty == 6 && dto.Qty >6)
                {
                    throw new Exception("總數量不可超過6張");
                } 
                
            }
            orderItemInDb.OrderId = dto.OrderId;
            orderItemInDb.TicketId = dto.TicketId;
            orderItemInDb.TicketName = dto.TicketName;
            orderItemInDb.Price = dto.Price;
            orderItemInDb.Qty = dto.Qty;
            orderItemInDb.SubTotal = dto.SubTotal;

            _dao.Edit(orderItemInDb);
        }

        public void Delete(int id)
        {
            var orderItemInDb = _dao.GetOrderItemById(id);

            if (orderItemInDb == null) throw new Exception("找不到此電影票!");

            _dao.Delete(orderItemInDb);
        }
    }
}
