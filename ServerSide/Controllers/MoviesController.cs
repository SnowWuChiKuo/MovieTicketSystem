using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.Interfaces;
using ServerSide.Models.Services;
using ServerSide.Models.ViewModels;
using System.Net;

namespace ServerSide.Controllers
{
    public class MoviesController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IMovieService _service;

        public MoviesController(AppDbContext db,IMovieService service)
        {
            _db = db;
            _service = service;
        }

        [HttpGet]
        public IActionResult Search(string title, int? genre, int? rating, DateTime? startDate, DateTime? endDate)
        {
            var indexData = _service.GetAll();
            if (!string.IsNullOrEmpty(title))
                indexData = indexData.Where(id => id.Title.Contains(title,StringComparison.OrdinalIgnoreCase)).ToList();

            if (genre.HasValue)
                indexData = indexData.Where(id => id.GenreId == genre.Value).ToList();

            if(rating.HasValue)
				indexData = indexData.Where(id => id.RatingId == rating.Value).ToList();

			if (startDate.HasValue)
				indexData = indexData.Where(id => id.ReleaseDate >= startDate.Value).ToList();

			if (endDate.HasValue)
				indexData = indexData.Where(id => id.ReleaseDate <= endDate.Value).ToList();

			ViewBag.GenresName = _service.GetGenresName();
			ViewBag.RatingsName = _service.GetRatingsName();
			return View("Index", indexData);
		}


        [HttpGet]
        public IActionResult Index()
        {
			ViewBag.GenresName = _service.GetGenresName();
			ViewBag.RatingsName = _service.GetRatingsName();
			var indexData = _service.GetAll();
			return View(indexData);
        }

        public IActionResult Create()
        {
            ViewBag.GenresName = _service.GetGenresName();
            ViewBag.RatingsName = _service.GetRatingsName();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MovieVm model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.GenresName = _service.GetGenresName();
                ViewBag.RatingsName = _service.GetRatingsName();
                return View(model);
            }
            MovieDto dto = ConvertToDTO(model);

            try
            {
                _service.Create(dto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("",ex.Message);
                ViewBag.GenresName = _service.GetGenresName();
                ViewBag.RatingsName = _service.GetRatingsName();
                return View(model);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
		public IActionResult Edit(int? id)
        {
            ViewBag.GenresName = _service.GetGenresName();
            ViewBag.RatingsName = _service.GetRatingsName();
			if (id == null)
			{
				return BadRequest("ID 不能為空。");
			}

			var movieDto = _service.FindMovieById(id.Value);
			if (movieDto == null)
			{
				return NotFound("找不到指定的電影。");
			}

            var movieVm = ConvertToVm(movieDto);

            return View(movieVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(MovieVm model)
        {
            if (!ModelState.IsValid)
            {
				//發生錯誤返回view時重新設置下拉選單資料，避免資料消失
				ViewBag.GenresName = _service.GetGenresName();
				ViewBag.RatingsName = _service.GetRatingsName();
				return View(model);
            }

            MovieDto dto = ConvertToDTO(model);

            try
            {
                _service.Edit(dto);
			}
            catch (Exception ex)
            {
				ViewBag.GenresName = _service.GetGenresName();
				ViewBag.RatingsName = _service.GetRatingsName();
				ModelState.AddModelError("",ex.Message);
				return View(model);
			}
            return RedirectToAction("Index");
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Delete(int id)
		{
            try
            {
                _service.Delete(id);

				return RedirectToAction("Index");
			}
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
				return View("Edit", new MovieVm { Id = id });
			}
		}

        public IActionResult List()
        {
			ViewBag.GenresName = _service.GetGenresName();
			ViewBag.RatingsName = _service.GetRatingsName();
			var indexData = _service.GetAll();
			return View(indexData);
		}

		/// <summary>
		/// 把傳回的DTO轉成ViewModel給ViewPage用
		/// </summary>
		/// <param name="dto"></param>
		/// <returns></returns>
		private MovieVm ConvertToVm(MovieDto dto)
		{
			return new MovieVm
			{
				Id = dto.Id,
				GenreId = dto.GenreId,
				RatingId = dto.RatingId,
				Title = dto.Title,
				Description = dto.Description,
				Director = dto.Director,
				Cast = dto.Cast,
				RunTime = dto.RunTime,
				ReleaseDate = dto.ReleaseDate,
				PosterUrl = dto.PosterUrl,
				TrailerUrl = dto.TrailerUrl,
				CreatedAt = dto.CreatedAt,
				Updated = dto.Updated
			};
		}

		/// <summary>
		/// 轉成DTO給Service、DAO使用
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		private MovieDto ConvertToDTO(MovieVm model)
        {
            return new MovieDto
            {
                Id = model.Id,
                GenreId = model.GenreId,
                RatingId = model.RatingId,
                Title = model.Title,
                Description = model.Description,
                Director = model.Director,
                Cast = model.Cast,
                RunTime = model.RunTime,
                ReleaseDate = model.ReleaseDate,
                PosterUrl = model.PosterUrl,
                TrailerUrl = model.TrailerUrl,
                CreatedAt = model.CreatedAt,
                Updated = model.Updated
            };
        }
    }
}
