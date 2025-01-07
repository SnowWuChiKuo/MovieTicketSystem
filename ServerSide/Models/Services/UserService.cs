using ServerSide.Models.DAOs;
using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.Infra;
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
        public void Create(UserDto model)
        {
            _dao.Create(model);
        }

        public UserDto Get(int id)
        {
            return _dao.Get(id);

        }
        public UserDto ConvertToDto(UserCreateVm model)
        {
            var dto = new UserDto
            {
                Account = model.Account,
                Email = model.Email,
                Password = model.Password,
                //PasswordHash = ,
                Name = model.Name,
                IsAdmin = model.IsAdmin,
            };
            dto.PasswordHash = HashUtility.ToSHA256(model.Password, HashUtility.GetSalt());

            return dto;
        }

        public void Edit(UserDto dto)
        {
            _dao.Edit(dto);
        }

        public void Delete(string account)
        {
            _dao.Delete(account);
        }

        public void ValidateLogin(string account, string password)
        {
            using (var db = new AppDbContext())
            {
                var member = db.Members.FirstOrDefault(m => m.Account == account);
                //若帳號找不到就拋出例外
                if (member == null) throw new Exception("帳號或密碼錯誤");

                //若密碼錯誤就拋出例外
                if (!HashUtility.VerifySHA256(password, member.PasswordHash)) throw new Exception("帳號或密碼錯誤");

            }
        }

        public (string userName, string role) ProcessLogin(string account)
        {
            using (var db = new AppDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Account == account);
                return (user.Account, user.IsAdmin ? "Admin" : "User");
            }
        }
    }
}
