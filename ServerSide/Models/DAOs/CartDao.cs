using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.ViewModels;

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
            var data = _db.Carts.Include(d => d.Member)
                                .Select(d => new CartVm
                                {
                                    Id = d.Id,
                                    MemberId = d.MemberId,
                                    MemberAccount = d.Member.Account,
                                    MemberName = d.Member.Name
                                }).ToList();
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
