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
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ScreeningDto GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ScreeningVm> GetScreeningList()
        {
            var indexData = _db.Screenings
                .Include(s => s.Movie)
                .Include(s => s.Theater)
                .Select(s => new ScreeningVm
                {
                    Id = s.Id,
                    MovieId = s.MovieId,
                    MovieTitle = s.Movie.Title,
                    TheaterId = s.TheaterId,
                    ScreeningDate = s.Movie.ReleaseDate,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    CreatedAt = s.CreatedAt
                }).ToList();
            return indexData;
        }
		public ScreeningEditVm GetEditList(int id)
		{
			// 取得特定場次資料
			var screening = _db.Screenings
				.Include(e => e.Movie)
				.Include(e => e.Theater)
				.FirstOrDefault(e => e.Id == id);

			if (screening == null) return null;

			// 取得所有電影資料並轉換為 SelectListItem
			var movieOptions = _db.Movies
				.Select(m => new SelectListItem
				{
					Value = m.Id.ToString(),
					Text = $"{m.Id} - {m.Title}",
					Selected = m.Id == screening.MovieId
				})
				.ToList();

			var editVm = new ScreeningEditVm
			{
				Id = screening.Id,
				MovieId = screening.MovieId,
				MovieTitle = screening.Movie.Title,
				TheaterId = screening.TheaterId,
				ScreeningDate = screening.Movie.ReleaseDate,
				StartTime = screening.StartTime,
				EndTime = screening.EndTime,
				MovieOptions = movieOptions
			};

			return editVm;
		}

		public bool IsScreeningExist(int id)
        {
            throw new NotImplementedException();
        }


        public void Edit(ScreeningDto dto)
        {
            throw new NotImplementedException();
        }


        public ScreeningDto ConvertToDto(Screening screening)
        {
            throw new NotImplementedException();
        }

        public Screening ConvertToEfEntity(ScreeningDto dto)
        {
            throw new NotImplementedException();
        }

		public IEnumerable<ScreeningEditVm> GetEditList()
		{
			throw new NotImplementedException();
		}
	}
}
