using Microsoft.AspNetCore.Mvc.Rendering;
using ServerSide.Models.DAOs;
using ServerSide.Models.DTOs;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.Services
{
    public class SeatService
    {
        private readonly SeatDao _dao;

        public SeatService(SeatDao dao)
        {
            _dao = dao;
        }

        public List<SeatVm> GetAll()
        {
            return _dao.GetAll();
        }



        public List<SelectListItem> GetTheatersName()
        {
            return _dao.GetTheatersName();
        }

        public List<SelectListItem> IsDisabledOptions()
        {
            return _dao.IsDisabledOptions();
        }

        public void Create(SeatDto dto)
        {
            _dao.Create(dto);
        }

        public SeatVm Get(int id)
        {
            return _dao.Get(id);
        }

        public void Edit(SeatDto dto)
        {
            var seatInDb = _dao.GetSeatById(dto.Id);

            if (seatInDb == null) throw new Exception("找不到此座位!");

            seatInDb.TheaterId = dto.TheaterId;
            seatInDb.Row = dto.Row;
            seatInDb.Number = dto.Number;
            seatInDb.IsDisabled = dto.IsDisabled;

            _dao.Edit(seatInDb);
        }

        public void Delete(int seatId)
        {
            var seatInDb = _dao.GetSeatById(seatId);
            
            if (seatInDb == null) throw new Exception("找不到此座位!");

            _dao.Delete(seatInDb);
        }
    }
}
