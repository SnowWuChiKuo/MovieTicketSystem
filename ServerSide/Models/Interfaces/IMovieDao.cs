using Microsoft.AspNetCore.Mvc.Rendering;
using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;

namespace ServerSide.Models.Interfaces
{
    public interface IMovieDao
    {
        void Create(MovieDto dto);
        void Edit(MovieDto dto);
		void Delete(int id);
        Movie ConvertToEfEntity(MovieDto dto);
		List<SelectListItem> GetGenresName();
		List<SelectListItem> GetRatingsName();
	}
}
