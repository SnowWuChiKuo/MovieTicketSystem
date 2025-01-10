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
            screeningInDb.Televising = dto.Televising;
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
        public bool IsValidTelevisingDate(int movieId, DateOnly televising)
        {
            var releaseDate = GetMovieReleaseDate(movieId);
            return televising.ToDateTime(TimeOnly.MinValue) >= releaseDate;
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
                    Televising = s.Televising,
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
            var screening = _db.Screenings
                .Include(e => e.Movie)
                .Include(e => e.Theater)
                .FirstOrDefault(e => e.Id == id);

            var movieOptions = _db.Movies
                .Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = $"{m.Id} - {m.Title}",
                    Selected = screening != null && m.Id == screening.MovieId
                })
                .ToList();

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
                Televising = screening.Televising,
                StartTime = screening.StartTime,
                RunTime = screening.Movie.RunTime,
                EndTime = screening.StartTime.AddMinutes(screening.Movie.RunTime.HasValue ? screening.Movie.RunTime.Value : 0),
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
                Televising = screening.Televising,
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
                Televising = dto.Televising,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            };
        }

	}
}
