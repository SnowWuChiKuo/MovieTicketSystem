using Microsoft.AspNetCore.Mvc;
using ServerSide.Models.DTOs;
using ServerSide.Models.Services;
using ServerSide.Models.ViewModels;

namespace ServerSide.Controllers
{
    public class CouponsController : Controller
    {
        private readonly CouponService _service;

        public CouponsController(CouponService service)
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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CouponVm model)
        {
            if (!ModelState.IsValid) return View(model);

            CouponDto dto = ConvertTODTO(model);

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

        private CouponDto ConvertTODTO(CouponVm model)
        {
            return new CouponDto
            {
                Id = model.Id,
                Code = model.Code,
                DiscountType = model.DiscountType,
                DiscountValue = model.DiscountValue,
                ExpirationDate = model.ExpirationDate,
            };
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var data = _service.Get(id);

            return View(data);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CouponVm model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            CouponDto dto = ConvertTODTO(model);

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
