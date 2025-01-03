﻿using ClientSide.Models.DTOs;
using ClientSide.Models.Services;
using ClientSide.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

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
        /// 已啟用帳號的通知頁面，url是 /Members/ActiveRegister?memberId=&confirmCode=
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
            try
            {
                var service = new MemberService();
                service.ValidateLogin(model.Account , model.Password);//若失敗會拋出例外

                (string returnUrl, HttpCookie cookie) = service.ProcessLogin(model.Account);

                //登入成功，設定cookie
                Response.Cookies.Add(cookie);

                //導向returnUrl
                return Redirect(returnUrl);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("" ,ex.Message);
                return View(model);
            }
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Members");
        }

        /// <summary>
        /// 會員中心頁(登入才可看)
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult EditProfile()
        {
            //取得個人基本資料
            string account = User.Identity.Name;

            var service = new MemberService();

            var model = service.GetProfileVm(account);

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(ProfileVm model)
        {
            if (!ModelState.IsValid) return View(model);

            string account = User.Identity.Name;

            var service = new MemberService();

            service.UpdateProfile(account, model);

            TempData["Message"] = "個人資料已更新";

            return RedirectToAction("Index");

        }



    }
}