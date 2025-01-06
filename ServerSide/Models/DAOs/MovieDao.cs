using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.Interfaces;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.DAOs
{
	public class MovieDao : IMovieDao
    {
        private readonly AppDbContext _db;
        public MovieDao(AppDbContext db)
        {
            _db = db;
        }

		public List<MovieVm> GetAll()
		{
            var indexData =
			_db.Movies.Include(m => m.Genre)
                .Include(m => m.Prices)
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
            return indexData;
		}

		public void Create(MovieDto dto)
        {
            var movie = ConvertToEfEntity(dto);
            _db.Movies.Add(movie);
            _db.SaveChanges();
        }

		public void Edit(MovieDto dto)
		{
            var movie = _db.Movies.Find(dto.Id);
			if (movie == null)
            {
                throw new Exception("該筆資料不存在或已被刪除!");
			}
            
			movie.GenreId = dto.GenreId;
			movie.RatingId = dto.RatingId;
			movie.Title = dto.Title;
			movie.Description = dto.Description;
			movie.Director = dto.Director;
			movie.Cast = dto.Cast;
			movie.RunTime = dto.RunTime;
			movie.ReleaseDate = dto.ReleaseDate;
			movie.PosterUrl = dto.PosterUrl;
			movie.TrailerUrl = dto.TrailerUrl;
			movie.Updated = dto.Updated;

			_db.SaveChanges();
		}

		public void Delete(int id)
		{
			var movieInDb = _db.Movies.Find(id);
            if (movieInDb == null)
            {
				throw new Exception("該筆資料不存在或已被刪除!");
			}

            _db.Movies.Remove(movieInDb);
            _db.SaveChanges();
		}

		public Movie ConvertToEfEntity(MovieDto dto)
        {
            return new Movie
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
                Updated = dto.Updated,
            };
        }

        /// <summary>
        /// 取得電影分類名稱，存成下拉選單Item
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetGenresName()
        {
            return _db.Genres
                .AsNoTracking()
                .Select(g => new SelectListItem
                    {
                        Value = g.Id.ToString(),
                        Text = g.Name
                    })
                .ToList();
        }

        /// <summary>
        /// 取得電影分級名稱，存成下拉選單Item
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetRatingsName()
        {
            return _db.Ratings
				.AsNoTracking()
				.Select(r => new SelectListItem
                    {
                        Value = r.Id.ToString(),
                        Text = r.Name
                })
                .ToList();
        }

		public MovieDto FindMovieById(int id)
		{
			var movieInDb = _db.Movies.Find(id);
			if (movieInDb == null)
			{
				throw new Exception("該筆資料不存在或已被刪除!");
			}
			var movieDto = ConvertToDTO(movieInDb);
			return movieDto;
		}

		public MovieDto ConvertToDTO(Movie movieInDb)
		{
			if (movieInDb == null)
			{
				throw new ArgumentNullException(nameof(movieInDb));
			}
			return new MovieDto
			{
				Id = movieInDb.Id,
				GenreId = movieInDb.GenreId,
				RatingId = movieInDb.RatingId,
				Title = movieInDb.Title,
				Description = movieInDb.Description,
				Director = movieInDb.Director,
				Cast = movieInDb.Cast,
				RunTime = movieInDb.RunTime,
				ReleaseDate = movieInDb.ReleaseDate,
				PosterUrl = movieInDb.PosterUrl,
				TrailerUrl = movieInDb.TrailerUrl,
				CreatedAt = movieInDb.CreatedAt,
				Updated = movieInDb.Updated
			};
		}


		//public MovieDto FindByTitle(string title)
		//{
		//    var movieInDb = _db.Movies
		//        .AsNoTracking()
		//        .Where(m => m.Title == title)
		//        .Select(m => new MovieDto
		//        {
		//            Id = m.Id,
		//            GenreId = m.GenreId,
		//            RatingId = m.RatingId,
		//            Title = m.Title,
		//            Description = m.Description,
		//            Director = m.Director,
		//            Cast = m.Cast,
		//            RunTime = m.RunTime,
		//            ReleaseDate = m.ReleaseDate,
		//            PosterUrl = m.PosterUrl,
		//            TrailerUrl = m.TrailerUrl,
		//            CreatedAt = m.CreatedAt,
		//            Updated = m.Updated
		//        })
		//        .FirstOrDefault();

		//    return movieInDb;
		//}


	}
}
