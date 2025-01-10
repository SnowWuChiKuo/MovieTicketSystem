using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerSide.Models.DTOs;
using ServerSide.Models.Services;
using ServerSide.Models.ViewModels;
using System.Security.Claims;

namespace ServerSide.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserService _service;
        public UsersController(UserService service)
        {
            _service = service;
        }

        [HttpGet("Users/Login")]
        public IActionResult Login(string returnUrl = "/")
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        [HttpPost("Users/Login")]
        [ValidateAntiForgeryToken]
        public IActionResult Login(UserLoginVm model, string returnUrl = "/")
        {
            ViewBag.ReturnUrl = returnUrl;

            if (!ModelState.IsValid) return View(model);
           
            try
            {
                _service.ValidateLogin(model.Account, model.Password); //若失敗會拋出例外

                (string userName, string role) = _service.ProcessLogin(model.Account);

                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, userName),
                        new Claim(ClaimTypes.Role, role)
                    };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal).Wait();

                return Redirect(returnUrl);
                
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var data = _service.GetAll();
            return View(data);
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(UserCreateVm model)
        {
            if (!ModelState.IsValid) return View(model);

            UserDto dto = _service.ConvertToDto(model);
            try
            {
                _service.Create(dto);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }

        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            UserDto userDto = _service.Get(id);
            UserCreateVm model = new UserCreateVm
            {
                Account = userDto.Account,
                Email = userDto.Email,
                Name = userDto.Name,
                IsAdmin = userDto.IsAdmin
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(UserCreateVm model)
        {
            if (!ModelState.IsValid) return View(model);

            UserDto dto = _service.ConvertToDto(model);

            try
            {
                _service.Edit(dto);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUser(UserCreateVm model)
        {
            //if (!ModelState.IsValid) return View(model);

            string account = model.Account;

            _service.Delete(account);

            TempData["ShowSuccessAlert"] = true;
            TempData["SweetAlertTitle"] = "員工帳號已刪除";
            TempData["SweetAlertText"] = "該員工帳號已成功刪除。";
            TempData["SweetAlertIcon"] = "success";

            // 如果刪的是本登入者的帳號就導回登入頁
            if (HttpContext.User.Identity.Name == account)
            {
                return RedirectToAction("Login");

            }
            // 如果刪的是其他人的帳號就導回員工列表頁
            else
            {
                return RedirectToAction("Index");
            }
        }

    }
}
