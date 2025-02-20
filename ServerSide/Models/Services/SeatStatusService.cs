﻿using ServerSide.Models.DAOs;
using ServerSide.Models.DTOs;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.Services
{
    public class SeatStatusService
    {
        private readonly SeatStatusDao _dao;
        public SeatStatusService(SeatStatusDao dao)
        {
            _dao = dao;
        }

        public List<SeatStatusVm> GetAll()
        {
            return _dao.GetAll();
        }

        public void Create(SeatStatusDto dto)
        {
            if (dto.Status == null) throw new Exception("請選擇座位狀態!");
            if (dto.Status != "可使用" || dto.Status != "不可使用") throw new Exception("請選擇可以用或不可用");
            _dao.Create(dto);
        }

        public SeatStatusVm Get(int id)
        {
            return _dao.Get(id);
        }

        public void Edit(SeatStatusDto dto)
        {
            var seatStatusInDb = _dao.GetSeatStatusById(dto.Id);

            if (seatStatusInDb == null) throw new Exception("找不到座位狀態!");

            if (dto.Status == null) throw new Exception("請選擇座位狀態!");

            seatStatusInDb.ScreeningId =  dto.ScreeningId;
            seatStatusInDb.SeatId = dto.SeatId;
            seatStatusInDb.Status = dto.Status;
            seatStatusInDb.UpdatedAt = DateTime.Now;

            _dao.Edit(seatStatusInDb);
        }

        public void Delete(int id) 
        {
            var seatStatusInDb = _dao.GetSeatStatusById(id);

            if (seatStatusInDb == null) throw new Exception("找不到此座位!");

            _dao.Delete(seatStatusInDb);
        }

    }
}
