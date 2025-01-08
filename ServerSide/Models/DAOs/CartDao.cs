using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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



    }
}
