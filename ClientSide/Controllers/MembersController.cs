using ClientSide.Models.DAOs;
using ClientSide.Models.DTOs;
using ClientSide.Models.Services;
using ClientSide.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;

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

        /// <summary>
        /// 登入頁
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 登出頁(登入才可看)
        /// </summary>
        /// <returns></returns>
        [Authorize]
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

        /// <summary>
        /// 編輯個人資料頁(登入才可看)
        /// </summary>
        /// <returns></returns>
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

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteMember(ProfileVm model)
        {
            string account = User.Identity.Name;

            var service = new MemberService();
            if (model.IsDeleted)
            {
                //有勾 checkbox
                service.Delete(account);
                TempData["Message"] = "會員資料已取消";
                return RedirectToAction("Logout", "Members");

            }
            else
            {
                TempData["Message"] = "請勾選取消會員身分";
                return RedirectToAction("EditProfile");
            }
        }

        /// <summary>
        /// 變更密碼頁(登入才可看)
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordVm model)
        {
            if (!ModelState.IsValid) return View(model);

            string account = User.Identity.Name;

            //直接叫用 MemberEFDao ，無透過service
            var dao = new MemberEFDao();

            try
            {
                dao.ChangePassword(account, model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("PasswordOrigin", ex.Message);
                return View(model);
            }

            TempData["Message"] = "密碼已變更";

            return RedirectToAction("Index");
        }

        /// <summary>
        /// 忘記密碼頁
        /// </summary>
        /// <returns></returns>
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordVm model)
        {
            if (!ModelState.IsValid) return View(model);

            var dao = new MemberEFDao();
            var member = new Models.EFModels.Member();
            try
            {
               member = dao.ProcessForgotPassword(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }

            //<自訂port>/Members/ResetPassword/?MemberId=&confirmCode=
            string url = Url.Action("ResetPassword", "Members", new { memberId = member.Id, confirmCode = member.ConfirmCode }, Request.Url.Scheme);

            //todo 寄送email



            return View("ConfirmForgotPassword");

          
        }
        /// <summary>
        /// 重設密碼頁，在忘記密碼頁申請成功後到信箱點確認信後導入該頁
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="confirmCode"></param>
        /// <returns></returns>
        public ActionResult ResetPassword(int memberId, string confirmCode)
        {
            var dao = new MemberEFDao();

            var member = dao.GetOfResetPassword(memberId, confirmCode);

            if (member == null) return View("ErrorResetPassword");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordVm model, int memberId, string confirmCode)
        {
            //欄位驗證失敗或confirmCode為空(並非從忘記密碼申請點信後來到這頁面)就返回
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(confirmCode)) return View(model);

            var dao = new MemberEFDao();
            var member = dao.GetOfResetPassword(memberId, confirmCode);
            if (member == null) return View("ErrorResetPassword");

            try
            {
                dao.ResetPassword(model, memberId, confirmCode);
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }

            TempData["Message"] = "密碼已重設";

            //回到登入頁
            return RedirectToAction("Login", "Members");
        }
    }
}