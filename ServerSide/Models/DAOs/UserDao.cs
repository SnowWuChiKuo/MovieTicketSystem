using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.DAOs
{
    public class UserDao
    {
        private readonly AppDbContext _db;
        public UserDao(AppDbContext db)
        {
            _db = db;
        }

        public List<UserIndexVm> GetAll()
        {
            var data = _db.Users.Select(u => new UserIndexVm
            {
                Id = u.Id,
                Account = u.Account,
                Email = u.Email,
                PasswordHash = u.PasswordHash,
                Name = u.Name,
                IsAdmin = u.IsAdmin
            });
            return data.ToList();
        }
        public void Create(UserDto model)
        {
            User user = new User
            {
                Account = model.Account,
                Email = model.Email,
                PasswordHash = model.PasswordHash,
                Name = model.Name,
                IsAdmin = model.IsAdmin
            };
            _db.Users.Add(user);
            _db.SaveChanges();
        }
        public UserDto Get(int id)
        {
            var userInDb = _db.Users.FirstOrDefault(u => u.Id == id);
            if (userInDb != null)
            {
                var data = new UserDto()
                {
                    Account = userInDb.Account,
                    Email = userInDb.Email,
                    PasswordHash = userInDb.PasswordHash,
                    Name = userInDb.Name,
                    IsAdmin = userInDb.IsAdmin,
                };
                return data;
            }
            else
            {
                throw new Exception("找不到該員工");
            }
        }

        public void Edit(UserDto dto)
        {
            var user = _db.Users.FirstOrDefault(u => u.Account == dto.Account);
            if (user != null)
            {
                user.Account = dto.Account;
                user.Email = dto.Email;
                user.Name = dto.Name;
                user.PasswordHash = dto.PasswordHash;
                user.IsAdmin = dto.IsAdmin;
                _db.SaveChanges();
            }
            else
            {
                throw new Exception("找不到該員工");
            }
        }

        public void Delete(string account)
        {
            var user = _db.Users.FirstOrDefault(u => u.Account == account);
            if (user != null)
            {
                _db.Remove(user);
                _db.SaveChanges();
            }
            else
            {
                throw new Exception("找不到該員工");
            }
        }
    }
}
