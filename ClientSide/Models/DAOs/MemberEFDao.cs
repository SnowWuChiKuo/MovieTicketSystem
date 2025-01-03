using ClientSide.Models.DTOs;
using ClientSide.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientSide.Models.DAOs
{
    public class MemberEFDao
    {
        public void Create(RegisterDto dto)
        {
            using (var db = new AppDbContext())
            {
                var member = new Member
                {
                    Account = dto.Account,
                    PasswordHash = dto.PasswordHash,
                    Name = dto.Name,
                    Email = dto.Email,
                    IsConfirmed = false,
                    ConfirmCode = Guid.NewGuid().ToString("N")
                };
                db.Members.Add(member);
                db.SaveChanges();
            }
        }

        public bool IsExists(string account)
        {
            using (var db = new AppDbContext())
            {
                return db.Members.Any(m => m.Account == account);
            }
        }
    }
}