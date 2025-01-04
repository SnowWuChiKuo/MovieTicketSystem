using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.Services;
using ServerSide.Models.ViewModels;
using System.Net;

namespace ServerSide.Controllers
{
    public class MoviesController : Controller
    {
        private readonly AppDbContext _db;
        private readonly MovieService _service;

        public MoviesController(AppDbContext db,MovieService service)
        {
            _db = db;
            _service = service;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var movies = _db.Movies
                .Include(m => m.Genre).
                Include(m => m.Prices)
                .Select(m => new MovieVm
                {
                    Id = m.Id,
                    GenreId = m.GenreId,
                    RatingId = m.RatingId,
                    Title = m.Title,
                    Description = m.Description,
                    Director = m.Director,
                    Cast = m.Cast,
                    RunTime = m.RunTime,
                    ReleaseDate = m.ReleaseDate,
                    PosterUrl = m.PosterUrl,
                    TrailerUrl = m.TrailerUrl,
                    CreatedAt = m.CreatedAt,
                    Updated = m.Updated
                })
                .ToList();
            return View(movies);
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

        public IActionResult Edit(int? id)
        {
            ViewBag.GenresName = _service.GetGenresName();
            ViewBag.RatingsName = _service.GetRatingsName();
            if (id == null)
            {
                return StatusCode((int)HttpStatusCode.BadRequest);
            }
            Movie movie = _db.Movies.Find(id);
            if (movie == null)
            {
                return NotFound();
            }

            var movieVm = new MovieVm
            {
                Id = movie.Id,
                GenreId = movie.GenreId,
                RatingId = movie.RatingId,
                Title = movie.Title,
                Description = movie.Description,
                Director = movie.Director,
                Cast = movie.Cast,
                RunTime = movie.RunTime,
                ReleaseDate = movie.ReleaseDate,
                PosterUrl = movie.PosterUrl,
                TrailerUrl = movie.TrailerUrl,
                CreatedAt = movie.CreatedAt,
                Updated = movie.Updated
            };

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
