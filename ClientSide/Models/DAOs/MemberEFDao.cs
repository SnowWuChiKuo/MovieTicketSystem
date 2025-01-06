using ClientSide.Models.DTOs;
using ClientSide.Models.EFModels;
using ClientSide.Models.ViewModels;
using ServerSide.Models.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Security.Principal;
using System.Web;
using System.Web.Helpers;

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
                var member = db.Members.FirstOrDefault(m => m.Id == memberId && m.ConfirmCode == confirmCode && m.IsConfirmed == false);
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
                    throw new Exception("原始密碼錯誤");
                }
            }
        }

        /// <summary>
        /// 判斷該帳號是否存在，email是否正確，以及是否已開通，有就返回member
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Member GetOfForgotPassword(ForgotPasswordVm model)
        {
            using (var db = new AppDbContext())
            {
                var member = db.Members.FirstOrDefault(m => m.Account == model.Account && m.Email == model.Email && m.IsConfirmed == true);
                return member;
            }
        }

        public Member ProcessForgotPassword(ForgotPasswordVm model)
        {
            using (var db = new AppDbContext())
            {
                var member = db.Members.FirstOrDefault(m => m.Account == model.Account && m.Email == model.Email && m.IsConfirmed == true);
                //判斷帳號是否存在，email是否正確，以及是否已開通
                if (member != null)
                {
                    //update confirmCode = guid 
                    member.ConfirmCode = Guid.NewGuid().ToString("N");

                    db.SaveChanges();

                    return member;
                }
                else
                {
                    throw new Exception("帳號或Email輸入錯誤或是帳號未能開通");
                }
            }
        }

        public  Member GetOfResetPassword(int memberId, string confirmCode)
        {
            using (var db = new AppDbContext())
            {
                var member = db.Members.FirstOrDefault(m => m.Id == memberId && m.ConfirmCode == confirmCode);
                return member;
            }
        }

        public void ResetPassword(ResetPasswordVm model, int memberId, string confirmCode)
        {
            using(var db = new AppDbContext())
            {
                var member = db.Members.FirstOrDefault(m => m.Id == memberId && m.ConfirmCode == confirmCode);
                //if (member == null) return View("ErrorResetPassword");
                if (member != null)
                {
                    member.PasswordHash = HashUtility.ToSHA256(model.Password, HashUtility.GetSalt());
                    member.ConfirmCode = null;
                    db.SaveChanges();
                }
                else
                {
                    throw new Exception("memberId或confirmCode錯誤");
                }
            }
        }

        public void Delete(string account)
        {
            using (var db = new AppDbContext())
            {
                var member = db.Members.FirstOrDefault(m => m.Account == account);
                if (member != null)
                {
                    member.IsDeleted = true;
                    db.SaveChanges();
                }

            }
        }
    }
}