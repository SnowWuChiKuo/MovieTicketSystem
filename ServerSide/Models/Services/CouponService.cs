using Humanizer;
using ServerSide.Models.DAOs;
using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.Services
{
    public class CouponService
    {
        private readonly CouponDao _dao;

        public CouponService(CouponDao dao)
        {
            _dao = dao;
        }

        public List<CouponVm> GetAll()
        {
            return _dao.GetAll();
        }

        public void Create(CouponDto dto)
        {
            _dao.Create(dto);
        }

        public CouponVm Get(int id)
        {
            return _dao.Get(id);
        }

        public void Edit(CouponDto dto)
        {
            var couponInDb = _dao.GetCouponById(dto.Id);

            if (couponInDb == null) throw new Exception("找不到此優惠卷");


            couponInDb.Code = dto.Code;
            couponInDb.DiscountType = dto.DiscountType;
            couponInDb.DiscountValue = dto.DiscountValue;
            couponInDb.UpdatedAt = DateTime.Now;

            _dao.Edit(couponInDb);

        }

        public void Delete(int id)
        {
            var couponInDb = _dao.GetCouponById(id);

            if (couponInDb == null) throw new Exception("找不到此優惠卷");

            _dao.Delete(couponInDb);
        }
    }
}
