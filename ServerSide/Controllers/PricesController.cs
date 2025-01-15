using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServerSide.Models.DTOs;
using ServerSide.Models.Interfaces;
using ServerSide.Models.ViewModels;

namespace ServerSide.Controllers
{
    [Authorize]
    public class PricesController : Controller
    {
        private readonly IPriceService _service;
        public PricesController(IPriceService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var indexData = _service.GetMovieTixList();
            return View(indexData);
        }

        [HttpGet]
        public IActionResult Details(int movieId,string movieTitle)
        {
            var detailData = _service.GetMovieTixDetail(movieId);
            ViewBag.MovieTitle = movieTitle;
            ViewBag.MovieId = movieId;

            return View(detailData);
        }

        [HttpGet]
        public IActionResult Create(int movieId,string movieTitle)
        {
            var IsMovieExist = _service.IsMovieExist(movieId);
            if (!IsMovieExist)
            {
                return NotFound("該電影不存在");
            }
            ViewBag.MovieIdAndTitle = $"{movieId} - {movieTitle}";
            
            var vm = new PriceVm { MovieId = movieId };
            PrepareSelectLists(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PriceVm vm)
        {
            if(!ModelState.IsValid) return View(vm);
            var priceDto = ConvertToDto(vm);

            try
            {
                _service.Create(priceDto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("",ex.Message);
                return View(vm);                
            }
            return RedirectToAction("Index");
        }

        private PriceDto ConvertToDto(PriceVm vm)
        {
            return new PriceDto
            {
                Id = vm.Id,
                MovieId = vm.MovieId,
                SalesType = vm.SalesType,
                TicketType = vm.TicketType,
                ReservedSeats = vm.ReservedSeats,
                Price1 = vm.PriceOfTicket
            };
        }


        [HttpGet]
        public IActionResult Edit(int? id, int movieId, string movieTitle)
        {
            if (id == null) throw new Exception("Id不能為空!");
            var priceDto = _service.FindPriceById(id.Value);
            if (priceDto == null) throw new Exception("找不到票券資料!");

            ViewBag.MovieId = movieId;
            ViewBag.MovieTitle = movieTitle;

            var priceVm = ConvertToVm(priceDto);
            PrepareSelectLists(priceVm);
            return View(priceVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PriceVm vm,string movieTitle)
        {
            if (!ModelState.IsValid) return View(vm);
            PriceDto dto= ConvertToDto(vm);
            try
            {
                _service.Edit(dto);

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(vm);
            }

            return RedirectToAction("Details", new { movieId = vm.MovieId, movieTitle });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            try
            {
                _service.Delete(id);
                var newListData = _service.GetMovieTixList();

                return View("Index", newListData);
			}
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Index");
			}
        }

        private PriceVm ConvertToVm(PriceDto priceDto)
        {
            return new PriceVm
            {
                Id = priceDto.Id,
                MovieId = priceDto.MovieId,
                SalesType = priceDto.SalesType,
                TicketType = priceDto.TicketType,
                ReservedSeats = priceDto.ReservedSeats,
                PriceOfTicket = priceDto.Price1
            };
        }

        private void PrepareSelectLists(PriceVm vm)
        {
            vm.SalesTypeItem = PriceVm.SalesTypeOptions
                .Select(x => new SelectListItem
                {
                    Value = x,
                    Text = x
                }).ToList();

            vm.TicketTypeOption = PriceVm.TicketTypeOptions
                .Select(x => new SelectListItem
                {
                    Value = x,
                    Text = x
                }).ToList();
        }
    }
}
