using Microsoft.AspNetCore.Mvc.Rendering;
using ServerSide.Models.DAOs;
using ServerSide.Models.DTOs;
using ServerSide.Models.Interfaces;
using System.Security.Cryptography;

namespace ServerSide.Models.Services
{
    public class MovieService : IMovieService
    {
        private readonly MovieDao _dao;
        public MovieService(MovieDao dao)
        {
            _dao = dao;
        }
        public void Create(MovieDto dto)
        {
            if (dto.CreatedAt == default(DateTime))
            {
                dto.CreatedAt = DateTime.Now;
            }
            dto.Updated = DateTime.Now;
            _dao.Create(dto);
        }

        public void Edit(MovieDto dto)
        {
            var movieInDb = _dao.FindByTitle(dto.Title)
        }

        public List<SelectListItem> GetGenresName()
        {
            return _dao.GetGenresName();
        }

        public List<SelectListItem> GetRatingsName()
        {
            return _dao.GetRatingsName();
        }
    }

}
