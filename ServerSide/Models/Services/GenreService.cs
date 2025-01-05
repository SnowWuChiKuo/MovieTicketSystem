using ServerSide.Models.DTOs;
using ServerSide.Models.Interfaces;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.Services
{
	public class GenreService : IGenreService
	{
		private readonly IGenreDao _dao;
		public GenreService(IGenreDao dao)
		{
			_dao = dao;
		}
		public void Create(GenreDto dto)
		{
			_dao.Create(dto);
		}

		public void Delete(int id)
		{
			_dao.Delete(id);
		}

		public void Edit(GenreDto dto)
		{
			_dao.Edit(dto);
		}

		public GenreDto FindGenreById(int id)
		{
			return _dao.FindGenreById(id);
		}

		public List<GenreVm> GetAll()
		{
			return _dao.GetAll();
		}
	}
}
