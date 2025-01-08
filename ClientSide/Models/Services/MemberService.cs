using ClientSide.Models.DAOs;
using ClientSide.Models.DTOs;
using ClientSide.Models.EFModels;
using ClientSide.Models.ViewModels;
using ServerSide.Models.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace ClientSide.Models.Services
{
    public class MemberService
    {
        private MemberEFDao dao;
        public MemberService()
        {
            dao = new MemberEFDao();
        }
        public void Register(RegisterDto dto)
        {
            var dao = new MemberEFDao();
            //判斷帳號是否存在
            if (dao.IsExists(dto.Account))
            {
                throw new Exception("帳號已存在");
            }

            //密碼加密
            dto.PasswordHash = HashUtility.ToSHA256(dto.Password, HashUtility.GetSalt());

            dao.Create(dto);

        }

        /// <summary>
        /// 處理會員註冊的函式
        /// </summary>
        /// <param name="model"></param>
        public void ProcessRegister(RegisterVm model)
        {
            RegisterDto dto = new RegisterDto
            {
                Name = model.Name,
                Account = model.Account,
                Password = model.Password,
                Email = model.Email,
            };
            Register(dto);
        }

        public void ProcessActiveRegister(int memberId, string confirmCode)
        {
            MemberEFDao dao = new MemberEFDao();

            dao.ProcessActiveRegister(memberId, confirmCode);
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

                //若還沒開通就拋出例外
                if (member.IsConfirmed == false) throw new Exception("帳號未能開通");

                //IsDeleted == true
                if (member.IsDeleted == true) throw new Exception("帳號已被刪除");

                //IsBlackList == true
                if (member.IsBlackList == true) throw new Exception("帳號在黑名單中");


            }
        }

        public (string returnUrl, HttpCookie cookie) ProcessLogin(string account)
        {
            var roles = string.Empty;
            //建立一張認證票
            var ticket =
                new FormsAuthenticationTicket(
                    1, //版本別，沒特別用處
                    account,
                    DateTime.Now, //發行日
                    DateTime.Now.AddDays(2), //到期日
                    false, //是否續存
                    roles, //userdata
                    "/" //cookie的位置
                );
            //將它加密
            string value = FormsAuthentication.Encrypt(ticket);
            //存入cookies
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, value);
            //取得return url
            var url = FormsAuthentication.GetRedirectUrl(account, true); //第二個引數沒有用處

            return (url, cookie);
        }

        public ProfileVm GetProfileVm(string account)
        {
            var dao = new MemberEFDao();
            Member member = dao.Get(account);
            ProfileVm model = new ProfileVm
            {
                Account = member.Account,
                Email = member.Email,
                Name = member.Name,
            };
            return model;
        }

        public void UpdateProfile(string account, ProfileVm model)
        {
            var dao = new MemberEFDao();
            dao.UpdateProfile(account, model);
        }

        public void Delete(string account)
        {
            var dao = new MemberEFDao();
            dao.Delete(account);
        }

        public Member GetByAccount(string account)
        {
            var dao = new MemberEFDao();
            return dao.Get(account);
        }
    }
}