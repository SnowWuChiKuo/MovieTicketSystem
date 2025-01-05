using Microsoft.AspNetCore.Mvc.Rendering;
using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.Interfaces
{
    public interface IMovieDao
    {
        void Create(MovieDto dto);
        void Edit(MovieDto dto);
		void Delete(int id);
		List<MovieVm> GetAll();
		MovieDto FindMovieById(int id);
		Movie ConvertToEfEntity(MovieDto dto);
		MovieDto ConvertToDTO(Movie movieInDb);
		List<SelectListItem> GetGenresName();
		List<SelectListItem> GetRatingsName();
	}
}
