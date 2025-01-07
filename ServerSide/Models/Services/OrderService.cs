using Humanizer;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServerSide.Models.DAOs;
using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.Services
{
    public class OrderService
    {
        private readonly OrderDao _dao;

        public OrderService(OrderDao dao)
        {
            _dao = dao;
        }

        public List<OrderVm> GetAll()
        {
            return _dao.GetAll();
        }

        public List<SelectListItem> GetMemberAccount()
        {
            return _dao.GetMemberAccount();
        }

        public List<SelectListItem> GetCouponName()
        {
            return _dao.GetCouponName();
        }

        public void Create(OrderDto dto)
        {
            _dao.Create(dto);
        }

        public OrderVm Get(int id)
        {
            return _dao.Get(id);
        }

        public void Edit(OrderDto dto)
        {
            var orderInDb = _dao.GetOrderById(dto.Id);

            if (orderInDb == null) throw new Exception("找不到此訂單!");

            orderInDb.MemberId = dto.MemberId;
            orderInDb.CouponId = dto.CouponId;
            orderInDb.TotalAmount = dto.TotalAmount;
            orderInDb.Status = dto.Status;
            orderInDb.DiscountPrice = dto.DiscountPrice;

            _dao.Edit(orderInDb);
        }

        public void Delete(int id)
        {
            var orderInDb = _dao.GetOrderById(id);

            if (orderInDb == null) throw new Exception("找不到此訂單!");

            _dao.Delete(orderInDb);
        }
    }
}
