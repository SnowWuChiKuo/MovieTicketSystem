using ClientSide.Models.DAOs;
using ClientSide.Models.DTOs;
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

    }
}