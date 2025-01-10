using Microsoft.AspNetCore.Mvc.Rendering;
using ServerSide.Models.DAOs;
using ServerSide.Models.DTOs;
using ServerSide.Models.Interfaces;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.Services
{
    public class ScreeningService : IScreeningService
    {
        private readonly IScreeningDao _dao;
        public ScreeningService(IScreeningDao dao)
        {
            _dao = dao;
        }
		public IEnumerable<ScreeningVm> GetScreeningList()
        {
            return _dao. GetScreeningList();
        }

        public ScreeningEditVm GetEditList(int id)
		{
			return _dao.GetEditList(id);
		}

        public List<SelectListItem> GetMovieOptions()
        {
            return _dao.GetMovieOptions();
        }

        public List<SelectListItem> GetTheaterOptions()
        {
            return _dao.GetTheaterOptions();
        }

        /// <summary>
        /// 抓取電影Id，使新建場次頁面可以動態抓取電影長度
        /// </summary>
        /// <param name="movieId"></param>
        /// <returns></returns>
        public int? GetMovieRunTime(int movieId)
        {
            return _dao.GetMovieRunTime(movieId);
        }

        public bool ValidateTelevisingDate(int movieId, DateOnly televising)
        {
            return _dao.IsValidTelevisingDate(movieId, televising);
        }

        public DateTime GetMovieReleaseDate(int movieId)
        {
            return _dao.GetMovieReleaseDate(movieId);
        }



        public void Create(ScreeningDto dto)
        {
            _dao.Create(dto);
        }

        public void Edit(ScreeningDto dto)
        {
            dto.UpdatedAt = DateTime.Now;
            _dao.Edit(dto);
        }

        public void Delete(int id)
        {
            _dao.Delete(id);
        }


        public bool IsScreeningExist(int id)
        {
            throw new NotImplementedException();
        }


    }
}
