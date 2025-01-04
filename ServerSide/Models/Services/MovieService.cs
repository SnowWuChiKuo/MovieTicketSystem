using Microsoft.AspNetCore.Mvc.Rendering;
using ServerSide.Models.DAOs;
using ServerSide.Models.DTOs;
using ServerSide.Models.Interfaces;
using System.Security.Cryptography;

namespace ServerSide.Models.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieDao _dao;
        public MovieService(IMovieDao dao)
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

		public void Delete(int id)
		{
			_dao.Delete(id);
		}

		public void Edit(MovieDto dto)
        {
            dto.Updated = DateTime.Now;
			_dao.Edit(dto);
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
