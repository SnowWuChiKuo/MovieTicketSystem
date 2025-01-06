using ClientSide.Models.DTOs;
using ClientSide.Models.EFModels;
using ClientSide.Models.ViewModels;
using ServerSide.Models.Infra;
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
                    IsDeleted = false,
                    IsBlackList = false,
                    ConfirmCode = Guid.NewGuid().ToString("N"),
                    CreatedAt = DateTime.Now
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

        public void ProcessActiveRegister(int memberId, string confirmCode)
        {
            using (var db = new AppDbContext())
            {
                //如果用memberId , confirmCode找不到任何紀錄就不做任何事
                var member =db.Members.FirstOrDefault(m=>m.Id == memberId && m.ConfirmCode == confirmCode && m.IsConfirmed == false);
                if (member == null) return;

                //Update isConfirmed = true , confirmCode = null

                member.IsConfirmed = true;
                member.ConfirmCode = null;

                db.SaveChanges();
                
            }
        }

        public Member Get(string account)
        {
            using (var db = new AppDbContext())
            {
                return db.Members.FirstOrDefault(m => m.Account == account);

            }
        }

        public void UpdateProfile(string account, ProfileVm model)
        {
            using (var db = new AppDbContext())
            {
                var member = db.Members.FirstOrDefault(m => m.Account == account);

                member.Name = model.Name;
                member.Email = model.Email;

                db.SaveChanges();
            }
        }

        public void ChangePassword(string account, ChangePasswordVm model)
        {
            using (var db = new AppDbContext())
            {
                var member = db.Members.First(m => m.Account == account);

                if (HashUtility.ToSHA256(model.PasswordOrigin, HashUtility.GetSalt()) == member.PasswordHash)
                {
                    string hashedPassword = HashUtility.ToSHA256(model.PasswordNew, HashUtility.GetSalt());

                    member.PasswordHash = hashedPassword;

                    db.SaveChanges();

                    //TempData["Message"] = "更改密碼成功";

                    //return RedirectToAction("Index");
                }
                else
                {
                    //ModelState.AddModelError("PasswordOrigin", "原始密碼錯誤");
                    //return View(model);
                    throw new Exception("原始密碼錯誤");
                }
            }
        }
    }
}