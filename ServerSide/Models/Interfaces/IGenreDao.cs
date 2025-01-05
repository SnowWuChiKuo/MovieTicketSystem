using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.Interfaces
{
	public interface IGenreDao
	{
		List<GenreVm> GetAll();
		void Create(GenreDto dto);
		void Edit(GenreDto dto);
		void Delete(int id);
		GenreDto FindGenreById(int id);
		GenreDto ConvertToDto(Genre genre);
		Genre ConvertToEFEntity(GenreDto dto);
	}
}
