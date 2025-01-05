using Microsoft.AspNetCore.Mvc.Rendering;
using ServerSide.Models.DTOs;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.Interfaces
{
    public interface IMovieService
    {
        void Create(MovieDto dto);
        void Edit(MovieDto dto);
		void Delete(int id);
		MovieDto FindMovieById(int id);
		List<MovieVm> GetAll();
		List<SelectListItem> GetGenresName();
        List<SelectListItem> GetRatingsName();
        
	}
}
