using ServerSide.Models.DAOs;
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
    }
}
