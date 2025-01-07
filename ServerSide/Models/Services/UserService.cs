
using ServerSide.Models.DAOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.Services
{
    public class UserService
    {
        private readonly UserDao _dao;
        public UserService(UserDao dao)
        {
            _dao = dao;
        }
        public List<UserIndexVm> GetAll()
        {
            return _dao.GetAll();
        }
    }
}
