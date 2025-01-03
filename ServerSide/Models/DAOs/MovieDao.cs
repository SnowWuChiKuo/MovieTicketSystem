using Microsoft.AspNetCore.Mvc.Rendering;
using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.Interfaces;

namespace ServerSide.Models.DAOs
{
    public class MovieDao : IMovieDao
    {
        private readonly AppDbContext _db;
        public MovieDao(AppDbContext db)
        {
            _db = db;
        }
        public void Create(MovieDto dto)
        {
            var movie = ConvertToEfEntity(dto);
            _db.Movies.Add(movie);
            _db.SaveChanges();
        }

        public Movie ConvertToEfEntity(MovieDto dto)
        {
            return new Movie
            {
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
                .Select(r => new SelectListItem
                    {
                        Value = r.Id.ToString(),
                        Text = r.Name
                })
                .ToList();
        }

        public void Edit(MovieDto dto)
        {
            throw new NotImplementedException();
        }

        //public MovieDto FindByTitle(string title)
        //{
        //    var movieInDb = _db.Movies.FirstOrDefault(t => t.Title == title);
        //    return movieInDb;
        //}
    }
}
