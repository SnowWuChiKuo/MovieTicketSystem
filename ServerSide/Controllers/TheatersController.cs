using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerSide.Models.DTOs;
using ServerSide.Models.Interfaces;
using ServerSide.Models.ViewModels;

namespace ServerSide.Controllers
{
    [Authorize]
    public class TheatersController : Controller
    {
        private readonly ITheaterService _service;
        public TheatersController(ITheaterService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var indexData = _service.GetTheatersWithSeatsData();
            return View(indexData);
        }

        [HttpGet]
        public IActionResult Search(string name)
        {
            var indexData = _service.GetTheatersWithSeatsData();
            if (!string.IsNullOrEmpty(name))
            { 
                indexData = indexData.Where(i => i.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList(); 
            }
            return View("Index", indexData);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id == null)
            {
                return BadRequest("ID為空。");
            }
            var theaterDto = _service.FindTheaterById(id.Value);
            if (theaterDto == null)
            {
                return NotFound("找不到該影廳。");
            }
            var theaterVm = ConvertToVm(theaterDto);
            return View(theaterVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TheaterVm vm)
        {
            if(!ModelState.IsValid) return View(vm);
            var theaterDto = ConvertToDto(vm);
            try
            {
                _service.EditTheater(theaterDto);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(vm);
            }
        }

        private TheaterDto ConvertToDto(TheaterVm vm)
        {
            return new TheaterDto
            {
                Id = vm.Id,
                Name = vm.Name,
                CreatedAt = vm.CreatedAt
            };
        }

        private TheaterVm ConvertToVm(TheaterDto theaterDto)
        {
            return _service.GetTheaterVmById(theaterDto.Id);
        }
    }
}
