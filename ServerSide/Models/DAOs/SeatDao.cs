using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.DAOs
{
    public class SeatDao
    {
        private readonly AppDbContext _db;
        public SeatDao(AppDbContext db)
        {
            _db = db;
        }

        public List<SeatVm> GetAll()
        {
            var data = _db.Seats.Include(d => d.Theater)
                        .Select(d => new SeatVm
                        {
                            Id = d.Id,
                            TheaterId = d.TheaterId,
                            Row = d.Row,
                            Number = d.Number,
                            IsDisabled = d.IsDisabled
                        }).ToList();
            return data;
        }



        public List<SelectListItem> GetTheatersName()
        {
            return _db.Theaters.AsNoTracking()
                               .Select(t => new SelectListItem
                               {
                                   Value = t.Id.ToString(),
                                   Text = t.Name
                               }).ToList();
        }

        public List<SelectListItem> IsDisabledOptions()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "是", Value = "true" },
                new SelectListItem { Text = "不是", Value = "false" }
            };
        }

        public void Create(SeatDto dto)
        {
            var seat = ConvertToEfEntity(dto);
            _db.Seats.Add(seat);
            _db.SaveChanges();
        }

        private Seat ConvertToEfEntity(SeatDto dto)
        {
            return new Seat
            {
                Id = dto.Id,
                TheaterId = dto.TheaterId,
                Row = dto.Row,
                Number = dto.Number,
                IsDisabled = dto.IsDisabled,
            };
        }

        public Seat GetSeatById(int id)
        {
            var data = _db.Seats.FirstOrDefault(d => d.Id == id);

            if (data == null) throw new Exception("找不到此位子");

            return data;
        }

        public SeatVm Get(int id)
        {
            var seat = _db.Seats.Include(s => s.Theater)
                        .FirstOrDefault(s => s.Id == id);

            if (seat == null)
            {
                throw new Exception("找不到此位子");
            }

            return new SeatVm
            {
                Id = seat.Id,
                TheaterId = seat.TheaterId,
                Row = seat.Row,
                Number = seat.Number,
                IsDisabled = seat.IsDisabled
            };
        }


        public void Edit(Seat seat)
        {
            _db.Seats.Update(seat);
            _db.SaveChanges();
        }

        public void Delete(Seat seat)
        {
            _db.Seats.Remove(seat);
            _db.SaveChanges();
        }
    }
}
