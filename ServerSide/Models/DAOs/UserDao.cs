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
    }
}
