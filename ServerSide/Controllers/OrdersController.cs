using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerSide.Models.DTOs;
using ServerSide.Models.Services;
using ServerSide.Models.ViewModels;

namespace ServerSide.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly OrderService _service;
        public OrdersController(OrderService service)
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
            ViewBag.MemberAccount = _service.GetMemberAccount();
            ViewBag.CouponName = _service.GetCouponName();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(OrderVm model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.MemberAccount = _service.GetMemberAccount();
                ViewBag.CouponName = _service.GetCouponName();
                return View(model);
            }

            OrderDto dto = ConvertToDTO(model);

            try
            {
                _service.Create(dto);
                return RedirectToAction("Index");
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.TheatersName = _service.GetMemberAccount();
                ViewBag.TheatersName = _service.GetCouponName();
                return View(model);
            }
        }

        private OrderDto ConvertToDTO(OrderVm model)
        {
            return new OrderDto
            {
                Id = model.Id,
                MemberId = model.MemberId,
                CouponId = model.CouponId,
                TotalAmount = model.TotalAmount,
                Status = model.Status,
                DiscountPrice = model.DiscountPrice,
            };
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.MemberAccount = _service.GetMemberAccount();
            ViewBag.CouponName = _service.GetCouponName();
            var data = _service.Get(id);

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(OrderVm model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.MemberAccount = _service.GetMemberAccount();
                ViewBag.CouponName = _service.GetCouponName();
                return View(model);
            }

            OrderDto dto = ConvertToDTO(model);

            try
            {
                _service.Edit(dto);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.TheatersName = _service.GetMemberAccount();
                ViewBag.TheatersName = _service.GetCouponName();
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
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
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }
    }
}
