using Microsoft.AspNetCore.Mvc.Rendering;
using ServerSide.Models.DAOs;
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
    }
}
