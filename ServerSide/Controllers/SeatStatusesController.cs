using Microsoft.AspNetCore.Mvc;
using ServerSide.Models.DTOs;
using ServerSide.Models.Services;
using ServerSide.Models.ViewModels;

namespace ServerSide.Controllers
{
    public class SeatStatusesController : Controller
    {
        private readonly SeatStatusService _service;

        public SeatStatusesController(SeatStatusService service)
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
        public IActionResult Create(SeatStatusVm model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            SeatStatusDto dto = ConvertToDTO(model);

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

        private SeatStatusDto ConvertToDTO(SeatStatusVm model)
        {
            return new SeatStatusDto
            {
                Id = model.Id,
                Status = model.Status,
                ScreeningId = model.ScreeningId,
                SeatId = model.SeatId,
                UpdatedAt = model.UpdatedAt,
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
        public IActionResult Edit(SeatStatusVm model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            SeatStatusDto dto = ConvertToDTO(model);

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
