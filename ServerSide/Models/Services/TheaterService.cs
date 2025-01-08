using ServerSide.Models.DTOs;
using ServerSide.Models.Interfaces;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.Services
{
    public class TheaterService : ITheaterService
    {
        private readonly ITheaterDao _dao;
        public TheaterService(ITheaterDao dao)
        {
            _dao = dao;
        }

        public IEnumerable<TheaterVm> GetTheatersWithSeatsData()
        {
           return _dao.GetTheatersWithSeatsData();
        }

        public void EditTheater(TheaterDto dto)
        {
             _dao.EditTheater(dto);
        }

        public TheaterDto FindTheaterById(int id)
        {
            return _dao.FindTheaterById(id);
        }

        public TheaterVm GetTheaterVmById(int id)
        {
            return _dao.GetTheaterVmById(id);
        }
    }
}
