using ClientSide.Models.DTOs;
using ClientSide.Models.Services;
using ClientSide.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientSide.Controllers
{
    public class MembersController : Controller
    {
        // GET: Members
        /// <summary>
        /// 會員註冊頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterVm model)
        {
            if (!ModelState.IsValid) return View(model);
            try
            {
                var service = new MemberService();
                service.ProcessRegister(model);
                //todo: 寄送驗證信

                return View("RegisterConfirm");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("",ex.Message);
                return View(model);
            }

        }

        /// <summary>
        /// 已啟用帳號的通知頁面
        /// </summary>
        /// <returns></returns>
        public ActionResult ActiveRegister(int memberId ,string confirmCode)
        {
            var service = new MemberService();

            service.ProcessActiveRegister(memberId , confirmCode);

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginVm model)
        {
            if(!ModelState.IsValid) return View(model);

            return View();
        }


    }
}