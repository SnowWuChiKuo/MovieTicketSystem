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

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(UserLoginVm model, string returnUrl = null)
        {
            if (!ModelState.IsValid) return View(model);
            try
            {
                _service.ValidateLogin(model.Account, model.Password);//若失敗會拋出例外

                (string userName, string role) = _service.ProcessLogin(model.Account);

                var claims = new List<Claim>
                    {
                      new Claim(ClaimTypes.Name, userName),
                        new Claim(ClaimTypes.Role, role)
                    };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { };

                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties).Wait();

                if (string.IsNullOrEmpty(returnUrl))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return Redirect(returnUrl);
                }
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
        public IActionResult Index()
        {
            var data = _service.GetAll();
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
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

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUser(UserCreateVm model)
        {
            if (!ModelState.IsValid) return View(model);

            string account = model.Account;

            _service.Delete(account);

            TempData["ShowSuccessAlert"] = true;
            TempData["SweetAlertTitle"] = "員工帳號已刪除";
            TempData["SweetAlertText"] = "該員工帳號已成功刪除。";
            TempData["SweetAlertIcon"] = "success";

            return RedirectToAction("EditProfile");
        }

    }
}
