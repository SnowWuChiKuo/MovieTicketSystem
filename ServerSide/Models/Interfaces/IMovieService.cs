using Microsoft.AspNetCore.Mvc.Rendering;
using ServerSide.Models.DTOs;

namespace ServerSide.Models.Interfaces
{
    public interface IMovieService
    {
        void Create(MovieDto dto);
        void Edit(MovieDto dto);
		void Delete(int id);
		List<SelectListItem> GetGenresName();
        List<SelectListItem> GetRatingsName();
        
	}
}
