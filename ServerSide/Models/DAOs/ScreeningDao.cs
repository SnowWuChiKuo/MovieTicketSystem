using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.Interfaces;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.DAOs
{
    public class ScreeningDao : IScreeningDao
    {
        private readonly AppDbContext _db;

        public ScreeningDao(AppDbContext db)
        {
            _db = db;
        }


        public void Create(ScreeningDto dto)
        {
            var entity = ConvertToEfEntity(dto);
            _db.Screenings.Add(entity);
            _db.SaveChanges();
        }

        public void Edit(ScreeningDto dto)
        {
            var screeningInDb = _db.Screenings.Find(dto.Id);
            if (screeningInDb == null)
            {
                throw new Exception("找不到資料");
            }

            screeningInDb.MovieId = dto.MovieId;
            screeningInDb.TheaterId = dto.TheaterId;
            screeningInDb.StartTime = dto.StartTime;
            screeningInDb.EndTime = dto.EndTime;
            screeningInDb.UpdatedAt = dto.UpdatedAt;

            _db.SaveChanges();
        }

        /// <summary>
        /// 取得電影片長，供Create頁的JQuery去Fetch後動態抓取片長
        /// </summary>
        /// <param name="movieId"></param>
        /// <returns></returns>
        public int? GetMovieRunTime(int movieId)
        {
            return _db.Movies
                .Where(m => m.Id == movieId)
                .Select(m => m.RunTime)
                .FirstOrDefault();
        }
        public DateTime GetMovieReleaseDate(int movieId)
        {
            return _db.Movies
                .Where(m => m.Id == movieId)
                .Select(m => m.ReleaseDate.Date)
                .FirstOrDefault();
        }
        public bool IsValidScreeningDate(int movieId, DateTime screeningDate)
        {
            var releaseDate = GetMovieReleaseDate(movieId);
            return screeningDate.Date >= releaseDate;
        }
        public void Delete(int id)
        {
            var screeningInDb = _db.Screenings.Find(id);
            if (screeningInDb == null)
            {
                throw new Exception("找不到場次，請確認此資料是否已被更動");
            }
            _db.Screenings.Remove(screeningInDb);
            _db.SaveChanges();
        }

        public ScreeningDto GetById(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 單獨取得電影資料，給Create頁產生選單
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetMovieOptions()
        {
            return _db.Movies
                .Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = $"{m.Id} - {m.Title}",
                }).ToList();
        }

        /// <summary>
        /// 單獨取得影廳資料，給Create頁產生選單
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetTheaterOptions()
        {
            return _db.Theaters
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Name,
                }).ToList();
        }

        /// <summary>
        /// 取得場次index頁面資料
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ScreeningVm> GetScreeningList()
        {
            var indexData = _db.Screenings
                .Include(s => s.Movie)
                .Include(s => s.Theater)
                .Where(s => !s.Movie.Title.Contains("預告"))
                .Select(s => new ScreeningVm
                {
                    Id = s.Id,
                    MovieId = s.MovieId,
                    MovieTitle = s.Movie.Title,
                    TheaterId = s.TheaterId,
                    ScreeningDate = s.Movie.ReleaseDate,
                    StartTime = s.StartTime,
                    EndTime = s.StartTime.AddMinutes(s.Movie.RunTime ?? 0),
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt
                }).ToList();
            return indexData;
        }

        /// <summary>
        /// 取得Edit頁面資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		public ScreeningEditVm GetEditList(int id)
		{
			// 取得特定場次資料
			var screening = _db.Screenings
				.Include(e => e.Movie)
				.Include(e => e.Theater)
				.FirstOrDefault(e => e.Id == id);

            // 取得所有電影資料並轉換為 SelectListItem，轉換為List
            var movieOptions = _db.Movies
                .Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = $"{m.Id} - {m.Title}",
                    Selected = screening != null && m.Id == screening.MovieId
                })
                .ToList();

            // 取得所有影廳資料轉換為 SelectListItem，轉換為List
            var theaterOptions = _db.Theaters
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Name,
                    Selected = screening != null && t.Id == screening.TheaterId
                })
                .ToList();

            if (screening == null)
            {
                return new ScreeningEditVm
                {
                    MovieOptions = movieOptions,
                    TheaterOptions = theaterOptions
                };
            }
            var editVm = new ScreeningEditVm
			{
				Id = screening.Id,
				MovieId = screening.MovieId,
				MovieTitle = screening.Movie.Title,
				TheaterId = screening.TheaterId,
				ScreeningDate = screening.Movie.ReleaseDate.Date,
				StartTime = screening.StartTime,
                //如果場次中的電影有值，該場次的結束時間就是把開始時間加上電影片長(int? RunTime)
                RunTime = screening.Movie.RunTime,
				EndTime = screening.StartTime.AddMinutes(screening.Movie.RunTime.HasValue ? screening.Movie.RunTime.Value : 0 ),
				MovieOptions = movieOptions,
                TheaterOptions = theaterOptions
			};

			return editVm;
		}

		public bool IsScreeningExist(int id)
        {
            throw new NotImplementedException();
        }


        public ScreeningDto ConvertToDto(Screening screening)
        {
            return new ScreeningDto
            {
                Id = screening.Id,
                MovieId = screening.MovieId,
                TheaterId = screening.TheaterId,
                StartTime = screening.StartTime,
                EndTime = screening.EndTime,
                CreatedAt = screening.CreatedAt,
                UpdatedAt = screening.UpdatedAt
            };
        }

        public Screening ConvertToEfEntity(ScreeningDto dto)
        {
            return new Screening
            {
                Id = dto.Id,
                MovieId = dto.MovieId,
                TheaterId = dto.TheaterId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            };
        }

	}
}
