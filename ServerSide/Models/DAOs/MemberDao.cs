using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.Infra;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.DAOs
{
    public class MemberDao
    {
        private readonly AppDbContext _db;

        public MemberDao(AppDbContext db)
        {
            _db = db;
        }


        public List<MemberIndexVm> GetAll()
        {
            var data = _db.Members.Select(m => new MemberIndexVm
            {
                Id = m.Id,
                Account = m.Account,
                Email = m.Email,
                PasswordHash = m.PasswordHash,
                Name = m.Name,
                IsDeleted = m.IsDeleted,
                IsConfirmed = m.IsConfirmed,
                IsBlackList = m.IsBlackList,
                ConfirmCode = m.ConfirmCode,
                CreatedAt = m.CreatedAt,
                UpdatedAt = m.UpdatedAt
            });

            return data.ToList();
        }
        public MemberDto Get(int id)
        {
            var memberInDb = _db.Members.FirstOrDefault(m => m.Id == id);
            var data = new MemberDto()
            { 
                Account = memberInDb.Account,
                Email = memberInDb.Email,
                PasswordHash = memberInDb.PasswordHash,
                Name= memberInDb.Name,
                IsDeleted =memberInDb.IsDeleted,
                IsConfirmed = memberInDb.IsConfirmed,
                IsBlackList = memberInDb.IsBlackList,
            };
            return data;
        }

        public void Create(MemberDto model)
        {
            var member = new Member
            {
                Account = model.Account,
                Email = model.Email,
                PasswordHash = model.PasswordHash,
                Name = model.Name,
                IsDeleted = model.IsDeleted,
                IsBlackList = model.IsBlackList,
                IsConfirmed = model.IsConfirmed,
                ConfirmCode = model.ConfirmCode,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt
            };
            _db.Members.Add(member);
            _db.SaveChanges();
        }

        public void Edit(MemberDto dto)
        {
            var member = _db.Members.FirstOrDefault(m => m.Account == dto.Account);
            member.Account = dto.Account;
            member.Email = dto.Email;
            member.Name = dto.Name;
            member.PasswordHash = dto.PasswordHash;
            member.IsDeleted = dto.IsDeleted;
            member.IsConfirmed = dto.IsConfirmed;
            member.IsBlackList = dto.IsBlackList;
            _db.SaveChanges();
        }
    }
}
