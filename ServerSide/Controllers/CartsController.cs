using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerSide.Models.DTOs;
using ServerSide.Models.Services;
using ServerSide.Models.ViewModels;

namespace ServerSide.Controllers
{
    [Authorize]
    public class CartsController : Controller
    {
        private readonly CartService _service;

        public CartsController(CartService service)
        {
            _service = service;
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
            ViewBag.MemberAccount = _service.GetMembersAccount();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CartVm model)
        {
            if (!ModelState.IsValid) return View(model);

            CartDto dto = ConvertToDTO(model);

            try
            {
                ViewBag.MemberAccount = _service.GetMembersAccount();
                _service.Create(dto);
                return RedirectToAction("Index");
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.MemberAccount = _service.GetMembersAccount();
                return View(model);
            }
        }

        private CartDto ConvertToDTO(CartVm model)
        {
            return new CartDto
            {
                Id = model.Id,
                MemberId = model.MemberId,
            };
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null) return BadRequest("找不到此Id");

            try
            {
                _service.Delete(id.Value);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.MemberAccount = _service.GetMembersAccount();
                return View();
            }
        }


        //[HttpGet]
        //public IActionResult Edit(int id)
        //{
        //    ViewBag.MemberAccount = _service.GetMembersAccount();
        //    var data = _service.Edit(id);
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit(CartVm model)
        //{
        //    if (!ModelState.IsValid) return View(model);

        //    CartDto dto = ConvertToDTO(model);

        //    try
        //    {
        //        ViewBag.MemberAccount = _service.GetMembersAccount();
        //        _service.Create(dto);
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError("", ex.Message);
        //        ViewBag.MemberAccount = _service.GetMembersAccount();
        //        return View(model);
        //    }
        //}

    }
}
