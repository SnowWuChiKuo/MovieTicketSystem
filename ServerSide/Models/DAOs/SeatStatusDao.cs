using Humanizer;
using Microsoft.EntityFrameworkCore;
using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.DAOs
{
    public class SeatStatusDao
    {
        private readonly AppDbContext _db;

        public SeatStatusDao(AppDbContext db)
        {
            _db = db;
        }

        public List<SeatStatusVm> GetAll()
        {
            var data = _db.SeatStatuses.Include(d => d.Screening)
                                        .Include(d => d.Seat)
                                        .Select(d => new SeatStatusVm
                                                {
                                                    Id = d.Id,
                                                    ScreeningId = d.ScreeningId,
                                                    SeatId = d.SeatId,
                                                    Status = d.Status,
                                                    UpdatedAt = d.UpdatedAt
                                        }).ToList();
            return data;
        }

        public void Create(SeatStatusDto dto)
        {
            var seatStatus = ConvertToEfEntity(dto);
            _db.SeatStatuses.Add(seatStatus);
            _db.SaveChanges();
        }

        private SeatStatus ConvertToEfEntity(SeatStatusDto dto)
        {
            return new SeatStatus
            {
                Id = dto.Id,
                ScreeningId = dto.ScreeningId,
                SeatId = dto.SeatId,
                Status = dto.Status,
                UpdatedAt = dto.UpdatedAt
            };
        }

        public SeatStatus GetSeatStatusById(int id)
        {
            var data = _db.SeatStatuses.FirstOrDefault(d => d.Id == id);

            if (data == null) throw new Exception("找不到此位子");

            return data;
        }

        public SeatStatusVm Get(int id)
        {
            var seatStatus = _db.SeatStatuses.FirstOrDefault(s => s.Id == id);

            if (seatStatus == null)
            {
                throw new Exception("找不到位子狀態");
            }

            return new SeatStatusVm
            {
                Id = seatStatus.Id,
                ScreeningId = seatStatus.ScreeningId,
                SeatId = seatStatus.SeatId,
                Status = seatStatus.Status,
                UpdatedAt = seatStatus.UpdatedAt
            };

        }

        public void Edit(SeatStatus seatStatus)
        {
            _db.SeatStatuses.Update(seatStatus);
            _db.SaveChanges();
        }

        public void Delete(SeatStatus seatStatus)
        {
            _db.SeatStatuses.Remove(seatStatus);
            _db.SaveChanges();
        }
    }
}
