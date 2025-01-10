using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.ViewModels;
using System.Linq;

namespace ServerSide.Models.DAOs
{
    public class CartDao
    {
        private readonly AppDbContext _db;

        public CartDao(AppDbContext db)
        {
            _db = db;
        }

        public List<CartVm> GetAll()
        {
            var data = _db.Carts.Include(c => c.Member).Include(c => c.CartItems)
           .Select(c => new CartVm
           {
               Id = c.Id,
               MemberId = c.MemberId,
               MemberAccount = c.Member.Account,
               MemberName = c.Member.Name,
               TotalPrice = c.CartItems.Sum(ci => ci.SubTotal), // 計算總和
               CreatedAt = c.CreatedAt
           })
           .ToList();
            return data;
        }

        public List<SelectListItem> GetMembersAccount()
        {
            var data = _db.Members.AsNoTracking()
                                    .Select(d => new SelectListItem
                                    {
                                        Value = d.Id.ToString(),
                                        Text = d.Account
                                    }).ToList();
            return data;
        }

        public Cart GetCartById(int id)
        {
            var data = _db.Carts.FirstOrDefault(d => d.Id == id);

            if (data == null) throw new Exception("找不到此購物車");

            return data;
        }

        public Member GetMemberById(int id)
        {
            var data = _db.Members.FirstOrDefault(d => d.Id == id);

            if (data == null) throw new Exception("找不到此會員!");

            return data;
        }

        public void Create(CartDto dto)
        {
            var data = ConverToEntity(dto);

            _db.Carts.Add(data);
            _db.SaveChanges();
        }

        private Cart ConverToEntity(CartDto dto)
        {
            return new Cart
            {
                Id = dto.Id,
                MemberId = dto.MemberId,
            };
        }

        public CartVm Get(int id)
        {
            var cart = _db.Carts.FirstOrDefault(c => c.Id == id);

            if (cart == null)
            {
                throw new Exception("找不到此購物車!");
            }

            return new CartVm
            {
                Id= cart.Id,
                MemberId= cart.MemberId,
            };
        }

        public void Edit(Cart cart)
        {
            _db.Carts.Add(cart);
            _db.SaveChanges();
        }

        public void Delete(Cart cart)
        {
            _db.Carts.Remove(cart);
            _db.SaveChanges();
        }
    }
}
