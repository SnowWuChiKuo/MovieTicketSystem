using Humanizer;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServerSide.Models.DAOs;
using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.Services
{
    public class CartService
    {
        private readonly CartDao _dao;

        public CartService(CartDao dao)
        {
            _dao = dao;
        }

        public List<CartVm> GetAll()
        {
            return _dao.GetAll();
        }

        public List<SelectListItem> GetMembersAccount()
        {
            return _dao.GetMembersAccount();
        }

        public void Create(CartDto dto)
        {
            var existingCart = _dao.GetAll().FirstOrDefault(c => c.MemberId == dto.MemberId);

            if (existingCart != null)
            {
                throw new Exception("已經有購物車了!");
            }

            var memberInDb = _dao.GetMemberById(dto.MemberId);
            if (memberInDb == null)
            {
                throw new Exception("會員不存在!");
            }

            _dao.Create(dto);
        }

        public CartVm Edit(int id)
        {
            return _dao.Get(id);
        }

        public void Edit(CartDto dto)
        {
            var cartInDb = _dao.GetCartById(dto.Id);

            if (cartInDb == null) throw new Exception("找不到此購物車!");

            cartInDb.MemberId = dto.MemberId;

            _dao.Edit(cartInDb);
        }

        public void Delete(int id)
        {
            var cartInDb = _dao.GetCartById(id);

            if (cartInDb == null) throw new Exception("找不到此購物車!");


            _dao.Delete(cartInDb);
        }
    }
}
