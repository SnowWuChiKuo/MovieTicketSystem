using ServerSide.Models.DTOs;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.Interfaces
{
	public interface IGenreService
	{
		void Create(GenreDto dto);
		void Edit(GenreDto dto);
		void Delete(int id);
		GenreDto FindGenreById(int id);
		List<GenreVm> GetAll();
	}
}
