using ClientSide.Models.DAOs;
using ClientSide.Models.DTOs;
using ClientSide.Models.ViewModels;
using ServerSide.Models.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
    }
}