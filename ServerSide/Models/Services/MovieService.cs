using Humanizer;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServerSide.Models.DAOs;
using ServerSide.Models.DTOs;
using ServerSide.Models.Interfaces;
using ServerSide.Models.ViewModels;
using System.Security.Cryptography;

namespace ServerSide.Models.Services
{
	public class MovieService : IMovieService
    {
		//依賴介面(抽象)型別 實現依賴反轉 降低耦合
		private readonly IMovieDao _dao;
        public MovieService(IMovieDao dao)
        {
            _dao = dao;
        }
        public void Create(MovieDto dto)
        {
            if(dto.Title == null)
            {
                throw new Exception("電影標題不能為空!");
			}
            if (dto.CreatedAt == default(DateTime))
            {
                dto.CreatedAt = DateTime.Now;
            }
            dto.Updated = DateTime.Now;
            _dao.Create(dto);
        }

		public void Delete(int id)
		{
			if (id <= 0)
			{
				throw new ArgumentException("id值為空或不正確");
			}
			_dao.Delete(id);
		}

		public void Edit(MovieDto dto)
        {
			dto.Updated = DateTime.Now;

			_dao.Edit(dto);
		}

		public MovieDto FindMovieById(int id)
		{
			return _dao.FindMovieById(id);
		}

		public List<MovieVm> GetAll()
		{
			return _dao.GetAll();
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
