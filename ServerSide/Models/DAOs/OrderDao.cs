using Humanizer;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.DAOs
{
    public class OrderDao
    {
        private readonly AppDbContext _db;

        public OrderDao(AppDbContext db)
        {
            _db = db;
        }

        public List<OrderVm> GetAll()
        {
            var data = _db.Orders.Include(d => d.Member)
                                .Include(d => d.Coupon)
                                .Select(d => new OrderVm
                                {
                                    Id = d.Id,
                                    MemberId = d.MemberId,
                                    MemberAccount = d.Member.Account,
                                    CouponId = d.CouponId,
                                    TotalAmount = d.TotalAmount,
                                    Status = d.Status,
                                    DiscountPrice = d.DiscountPrice,
                                    CreatedAt = d.CreatedAt
                                }).ToList();
            return data;
        }

        public List<SelectListItem> GetMemberAccount()
        {
            return _db.Members.AsNoTracking()
                               .Select(t => new SelectListItem
                               {
                                   Value = t.Id.ToString(),
                                   Text = t.Account
                               }).ToList();
        }

        public List<SelectListItem> GetCouponName()
        {
            return _db.Coupons.AsNoTracking()
                               .Select(t => new SelectListItem
                               {
                                   Value = t.Id.ToString(),
                                   Text = t.Name
                               }).ToList();
        }

        public void Create(OrderDto dto)
        {
            var discountPrice = _db.Coupons.FirstOrDefault(d => d.Id == dto.CouponId);

            var order = ConvertTOEfEntity(dto);
            _db.Orders.Add(order);
            _db.SaveChanges();
        }

        private Order ConvertTOEfEntity(OrderDto dto)
        {
            return new Order
            {
                Id = dto.Id,
                MemberId = dto.MemberId,
                CouponId = dto.CouponId,
                TotalAmount = dto.TotalAmount,
                Status = dto.Status,
                DiscountPrice = dto.DiscountPrice
            };
        }

        public Order GetOrderById(int id)
        {
            var data = _db.Orders.FirstOrDefault(d => d.Id == id);

            if (data == null)
            {
                throw new Exception("找不到此訂單");
            }

            return data;
        }

        public OrderVm Get(int id)
        {
            var data = _db.Orders.FirstOrDefault(d => d.Id == id);

            if (data == null)
            {
                throw new Exception("找不到此訂單");
            }

            return new OrderVm
            {
                Id = data.Id,
                MemberId = data.MemberId,
                CouponId = data.CouponId,
                TotalAmount = data.TotalAmount,
                Status = data.Status,
                DiscountPrice = data.DiscountPrice
            };
        }

        public void Edit(Order order)
        {
            _db.Orders.Update(order);
            _db.SaveChanges();
        }

        public void Delete(Order order)
        {
            _db.Orders.Remove(order);
            _db.SaveChanges();
        }

        
    }
}
