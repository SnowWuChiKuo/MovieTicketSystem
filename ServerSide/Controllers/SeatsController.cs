using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.Services;
using ServerSide.Models.ViewModels;

namespace ServerSide.Controllers
{
    [Authorize]
    public class SeatsController : Controller
    {
        private readonly SeatService _service;
        public SeatsController(SeatService service)
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
            ViewBag.TheatersName = _service.GetTheatersName();
            ViewBag.IsDisabledOptions = _service.IsDisabledOptions();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SeatVm model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TheatersName = _service.GetTheatersName();
                ViewBag.IsDisabledOptions = _service.IsDisabledOptions();
                return View(model);
            }

            SeatDto dto = ConvertToDTO(model);

            try
            {
                _service.Create(dto);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.TheatersName = _service.GetTheatersName();
                ViewBag.IsDisabledOptions = _service.IsDisabledOptions();
                return View(model);
            }
        }

        private SeatDto ConvertToDTO(SeatVm model)
        {
            return new SeatDto
            {
                Id = model.Id,
                TheaterId = model.TheaterId,
                Row = model.Row,
                Number = model.Number,
                IsDisabled = model.IsDisabled,
            };
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.TheatersName = _service.GetTheatersName();
            ViewBag.IsDisabledOptions = _service.IsDisabledOptions();

            var data = _service.Get(id);

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(SeatVm model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TheatersName = _service.GetTheatersName();
                ViewBag.IsDisabledOptions = _service.IsDisabledOptions();
                return View(model);
            }

            SeatDto dto = ConvertToDTO(model);
            
            try
            {
                _service.Edit(dto);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Delete(int? id) 
        { 
            if (!ModelState.IsValid) return View();

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
