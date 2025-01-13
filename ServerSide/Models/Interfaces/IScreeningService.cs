using Microsoft.AspNetCore.Mvc.Rendering;
using ServerSide.Models.DTOs;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.Interfaces
{
    public interface IScreeningService
    {
        
        
        public IEnumerable<ScreeningVm> GetScreeningList();
        public List<SelectListItem> GetMovieOptions();
        public List<SelectListItem> GetTheaterOptions();
        public int? GetMovieRunTime(int movieId);

        public bool HasTimeConflict(int theaterId, DateOnly date, TimeOnly startTime, TimeOnly endTime, int excludeId = 0);

		bool ValidateTelevisingDate(int movieId, DateOnly televising);
        DateTime GetMovieReleaseDate(int movieId);


        public ScreeningEditVm GetEditList(int id);
		public bool IsScreeningExist(int id);
        public void Create(ScreeningDto dto);
        public void Edit(ScreeningDto dto);
        public void Delete(int id);
    }
}
