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
    public class CartItemsController : Controller
    {
        private readonly CartItemService _service;
        public CartItemsController(CartItemService service)
        {
            _service = service;
        }

        [HttpGet]
        //[Authorize]
        public IActionResult Index()
        {
            var data = _service.GetAll();
            return View(data);
        }


        //    [HttpGet]
        //    [Authorize(Roles = "Admin")]
        //    public IActionResult Create()
        //    {
        //        return View();
        //    }

        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    [Authorize(Roles = "Admin")]
        //    public IActionResult Create(UserCreateVm model)
        //    {
        //        if (!ModelState.IsValid) return View(model);

        //        UserDto dto = _service.ConvertToDto(model);
        //        try
        //        {
        //            _service.Create(dto);
        //            return RedirectToAction("Index");
        //        }
        //        catch (Exception ex)
        //        {
        //            ModelState.AddModelError("", ex.Message);
        //            return View(model);
        //        }

        //    }

        //    [HttpGet]
        //    [Authorize(Roles = "Admin")]
        //    public IActionResult Edit(int id)
        //    {
        //        UserDto userDto = _service.Get(id);
        //        UserCreateVm model = new UserCreateVm
        //        {
        //            Account = userDto.Account,
        //            Email = userDto.Email,
        //            Name = userDto.Name,
        //            IsAdmin = userDto.IsAdmin
        //        };
        //        return View(model);
        //    }

        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    [Authorize(Roles = "Admin")]
        //    public IActionResult Edit(UserCreateVm model)
        //    {
        //        if (!ModelState.IsValid) return View(model);

        //        UserDto dto = _service.ConvertToDto(model);

        //        try
        //        {
        //            _service.Edit(dto);
        //            return RedirectToAction("Index");
        //        }
        //        catch (Exception ex)
        //        {
        //            ModelState.AddModelError("", ex.Message);
        //            return View(model);
        //        }

        //    }

        //    [Authorize(Roles = "Admin")]
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public ActionResult DeleteUser(UserCreateVm model)
        //    {
        //        if (!ModelState.IsValid) return View(model);

        //        string account = model.Account;

        //        _service.Delete(account);

        //        TempData["ShowSuccessAlert"] = true;
        //        TempData["SweetAlertTitle"] = "員工帳號已刪除";
        //        TempData["SweetAlertText"] = "該員工帳號已成功刪除。";
        //        TempData["SweetAlertIcon"] = "success";

        //        return RedirectToAction("EditProfile");
        //    }
    }
}
