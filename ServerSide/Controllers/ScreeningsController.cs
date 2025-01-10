using Microsoft.AspNetCore.Mvc;
using ServerSide.Models.DTOs;
using ServerSide.Models.Interfaces;
using ServerSide.Models.ViewModels;
using MovieTicketSystem.Models.Attributes;
namespace ServerSide.Controllers
{
    public class ScreeningsController : Controller
    {
        private readonly IScreeningService _service;
        public ScreeningsController(IScreeningService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var indexData = _service.GetScreeningList();
            return View(indexData);
        }


		[HttpGet]
		public IActionResult Edit(int? id)
		{
			if (id == null)
			{
				return BadRequest("資料已被更動或被刪除");
			}

			var editVm = _service.GetEditList(id.Value);

			if (editVm == null)
			{
				return NotFound("找不到資料");
			}
            return View(editVm);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(ScreeningEditVm vm)
		{
            if (!ModelState.IsValid)
            {

                var editVm = _service.GetEditList(vm.Id);
                vm.MovieOptions = editVm.MovieOptions;
                vm.TheaterOptions = editVm.TheaterOptions;

                return View(vm);
            }
            var screeningDto = ConvertToDto(vm);

            try
            {
                _service.Edit(screeningDto);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                var editVm = _service.GetEditList(vm.Id);
                // 重新取得下拉選單資料，避免回去沒東西可以選
                vm.MovieOptions = editVm.MovieOptions;
                vm.TheaterOptions = editVm.TheaterOptions;
                return View(vm);
            }
        }

        [HttpGet]
        public IActionResult Search(int? theaterId,string movieTitle,TimeOnly? startTime,TimeOnly? endTime)
        {
            var indexData = _service.GetScreeningList();
            if (theaterId.HasValue)
                indexData = indexData.Where(i => i.TheaterId == theaterId).ToList();
            if (!string.IsNullOrEmpty(movieTitle))
                indexData = indexData.Where(i => i.MovieTitle.Contains(movieTitle, StringComparison.OrdinalIgnoreCase)).ToList();
            if (startTime.HasValue)
                indexData = indexData.Where(i => i.StartTime >= startTime).ToList();
            if (endTime.HasValue)
                indexData = indexData.Where(i => i.EndTime <= endTime).ToList();
            return View("Index", indexData);
        }

        public IActionResult Create()
        {
            var editVmForCreate = new ScreeningEditVm
            {
                MovieOptions = _service.GetMovieOptions(),
                TheaterOptions = _service.GetTheaterOptions()
            };
            return View(editVmForCreate);
        }

        /// <summary>
        /// Create的JQuery會Fetch這支，調用Dao中的取得片長
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetMovieRunTime(int movieId)
        {
            try
            {
                var runTime = _service.GetMovieRunTime(movieId);
                if (runTime.HasValue)
                {
                    return Json(runTime.Value);
                }
                return NotFound($"找不到電影ID {movieId} 的片長資訊");
            }
            catch (Exception ex)
            {
                return BadRequest($"取得片長時發生錯誤: {ex.Message}");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ScreeningEditVm vm)
        {
            if (!ModelState.IsValid)
            {
                var editVm = _service.GetEditList(vm.Id);
                // 重新取得下拉選單資料，避免回去沒東西可以選
                vm.MovieOptions = editVm.MovieOptions;
                vm.TheaterOptions = editVm.TheaterOptions;
                return View(vm);
            }

            var screeningDto = ConvertToDto(vm);

            try
            {
                if (!_service.ValidateTelevisingDate(vm.MovieId, vm.Televising))
                {
                    var releaseDate = _service.GetMovieReleaseDate(vm.MovieId);
                    ModelState.AddModelError("ScreeningDate",
                        $"場次日期不能早於電影上檔日期 {releaseDate:yyyy/MM/dd}");
                    return View(vm);
                };
                _service.Create(screeningDto);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                var editVm = _service.GetEditList(vm.Id);
                // 重新取得下拉選單資料，避免回去沒東西可以選
                vm.MovieOptions = editVm.MovieOptions;
                vm.TheaterOptions = editVm.TheaterOptions;
                return View(vm);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
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
                ModelState.AddModelError("",ex.Message);
                return RedirectToAction("Index");
            }
        }



        private ScreeningDto ConvertToDto(ScreeningEditVm vm)
        {
            return new ScreeningDto
			{
				Id = vm.Id,
                MovieId = vm.MovieId,
                MovieTitle = vm.MovieTitle,
                TheaterId = vm.TheaterId,
                Televising = vm.Televising,
                StartTime = vm.StartTime,
                EndTime = vm.EndTime,
            };
        }
    }
}
