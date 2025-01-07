using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.DAOs
{
    public class CouponDao
    {
        private readonly AppDbContext _db;

        public CouponDao(AppDbContext _db)
        {
            this._db = _db;
        }

        public List<CouponVm> GetAll()
        {
            var data = _db.Coupons.Select(d => new CouponVm
            {
                Id = d.Id,
                Name = d.Name,
                Code = d.Code,
                DiscountType = d.DiscountType,
                DiscountValue = d.DiscountValue,
                ExpirationDate = d.ExpirationDate,
                CreatedAt = d.CreatedAt,
                UpdatedAt = d.UpdatedAt
            }).ToList();

            return data;
        }


        public void Create(CouponDto dto)
        {
            Coupon coupon = ConvertTOEntity(dto);
            _db.Coupons.Add(coupon);
            _db.SaveChanges();
        }

        private Coupon ConvertTOEntity(CouponDto dto)
        {
            return new Coupon
            {
                Id = dto.Id,
                Name = dto.Name,
                Code = dto.Code,
                DiscountType = dto.DiscountType,
                DiscountValue = dto.DiscountValue,
                ExpirationDate = dto.ExpirationDate,
            };
        }

        public Coupon GetCouponById(int id)
        {
            var data = _db.Coupons.FirstOrDefault(d => d.Id == id);

            if (data == null) throw new Exception("找不到此優惠卷");

            return data;
        }


        public CouponVm Get(int id)
        {
            var coupon = _db.Coupons.FirstOrDefault(c => c.Id == id);

            if (coupon == null)
            {
                throw new Exception("找不到此優惠卷");
            }

            return new CouponVm
            {
                Id = coupon.Id,
                Name = coupon.Name,
                Code = coupon.Code,
                DiscountType = coupon.DiscountType,
                DiscountValue = coupon.DiscountValue,
                ExpirationDate = coupon.ExpirationDate,
            };
        }

        public void Edit(Coupon coupon)
        {
            _db.Coupons.Update(coupon);
            _db.SaveChanges();
        }

        public void Delete(Coupon coupon)
        {
            _db.Coupons.Remove(coupon);
            _db.SaveChanges();
        }
    }
}
