using ClientSide.Models.EFModels;
using ClientSide.Models.Repository;
using ClientSide.Models.Services;
using ClientSide.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ClientSide.Models.DTOs;
using System.Web.Mvc;

namespace ClientSide.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MovieService _service;
		public MoviesController()
		{
            var db = new AppDbContext();
			_service = new MovieService();
		}

		// GET: Movies
		public ActionResult Index()
        {
			var latestMovies = _service.GetLatestMovies();
			
			var mostReviewedMovies = _service.GetMostReviewedMovies();
			var upcomingMovies = _service.GetUpcomingMovies();

			var indexViewModel = new MovieIndexListVm
			{
				LatestMovies = latestMovies.Select(dto => new MovieIndexVm
				{
					Id = dto.Id,
					Title = dto.Title,
					ReleaseDate = dto.ReleaseDate,
					PosterURL = $"/MoviePosters/{dto.Title}.jpg",
					ReviewCount = dto.ReviewCount
				}).ToList(),
				MostReviewedMovies = mostReviewedMovies.Select(dto => new MovieIndexVm
				{
					Id = dto.Id,
					Title = dto.Title,
					ReleaseDate = dto.ReleaseDate,
					PosterURL = $"/MoviePosters/{dto.Title}.jpg",
					ReviewCount = dto.ReviewCount
				}).ToList(),
				UpcomingMovies = upcomingMovies.Select(dto => new MovieIndexVm
				{
					Id = dto.Id,
					Title = dto.Title,
					ReleaseDate = dto.ReleaseDate,
					PosterURL = $"/MoviePosters/{dto.Title}.jpg",
					ReviewCount = dto.ReviewCount
				}).ToList()
			};

			return View(indexViewModel);
		}

		

		/// <summary>
		/// 單一電影頁面
		/// </summary>
		/// <param name="movieId"></param>
		/// <returns></returns>
        [HttpGet]
		public ActionResult Movie(int movieId)
        {
            var movie = _service.GetMovieById(movieId);
            if (movie == null) return HttpNotFound();

			//看使用者是否登入，並儲存狀態
			movie.IsLoggedIn = User.Identity.IsAuthenticated;
            if (movie.IsLoggedIn)
            {
				//有登入就取帳號、檢查是否存在有效訂單
                var account = User.Identity.Name;
				movie.CanReview = _service.CheckUserHasValidOrder(account, movieId);
			}

            movie.ReleaseDate = movie.ReleaseDate.Date.ToUniversalTime();

            movie.PosterURL = $"/MoviePosters/{movie.Title}.jpg";

			if (movie.AverageRating.HasValue)
			{
				movie.AverageRating = Math.Round(movie.AverageRating.Value, 1);
			}

            var detailvm = new MovieDetailVm
            {
				Id = movie.Id,
				Title = movie.Title,
				Description = movie.Description,
				Director = movie.Director,
				Cast = movie.Cast,
				RunTime = movie.RunTime,
				ReleaseDate = movie.ReleaseDate,
				PosterURL = movie.PosterURL,
				GenreName = movie.GenreName,
				RatingName = movie.RatingName,
				ReviewCount = movie.ReviewCount,
				AverageRating = movie.AverageRating ?? 0,
				CanReview = movie.CanReview,
				IsLoggedIn = movie.IsLoggedIn,
				CurrentUserName = movie.CurrentUserName,
				Reviews = movie.Reviews
			};

			return View(movie);
		}



		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public ActionResult SubmitReview(int movieId,ReviewSubmitVm model)
		{
			if (!ModelState.IsValid)
			{
				return Json(new { success = false,message = "請確認評分是否填寫!"});
			}

			try
			{
				var account = User.Identity.Name;
				if (!_service.CheckUserHasValidOrder(account, movieId))
				{
					return Json(new { success = false, message = "您沒有本電影的訂票紀錄!" });
				}
				//新增評論，把傳來的頁面電影Id跟提交表單資料用Dto傳入
				var review = _service.AddReview(account ,new ReviewCreateDto
				{
					MovieId = movieId,
					Comment = model.Comment,
					Rating = model.Rating
				});

				return Json(new { 
					success = true,
					data = new{
						id = review.Id,
						memberName = review.MemberName,
						comment = review.Comment,
						createdAt = review.CreatedAt,
						rating = review.Rating
					}
				});

			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = "評論發表失敗" });
			}


		}

	}
}