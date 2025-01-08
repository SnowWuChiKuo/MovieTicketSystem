using Microsoft.EntityFrameworkCore;
using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.Interfaces;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.DAOs
{
    public class TheaterDao : ITheaterDao
    {

        private readonly AppDbContext _db;
        public TheaterDao(AppDbContext db)
        {
            _db = db;
        }
        public IEnumerable<TheaterVm> GetTheatersWithSeatsData()
        {
            var indexData = _db.Theaters.Include(i => i.Seats).Select(i => new TheaterVm
            {
                Id = i.Id,
                Name = i.Name,
                TotalSeats = i.Seats.Count,
                CreatedAt = i.CreatedAt
            }).ToList();
            if (indexData == null)
            {
                throw new Exception("影廳不存在!");
            }
            return indexData;
        }

        public void EditTheater(TheaterDto dto)
        {
            var theaterInDb = _db.Theaters.Find(dto.Id);
            if(theaterInDb == null)
            {
                throw new Exception("影廳不存在!");
            }
            theaterInDb.Name = dto.Name;
            _db.SaveChanges();
        }
        public TheaterDto ConvertToDto(Theater entity)
        {
            return new TheaterDto
            {
                Id = entity.Id,
                Name = entity.Name,
                CreatedAt = entity.CreatedAt
            };
        }

        public Theater ConvertToEfEntity(TheaterDto dto)
        {
            return new Theater
            {
                Id = dto.Id,
                Name = dto.Name,
                CreatedAt = dto.CreatedAt
            };
        }

        public TheaterDto FindTheaterById(int id)
        {
            var theaterInDb = _db.Theaters.Find(id);
            if (theaterInDb == null)
            {
                throw new Exception("影廳不存在!");
            }
            var theaterDto = ConvertToDto(theaterInDb);
            return theaterDto;
        }

        public TheaterVm GetTheaterVmById(int id)
        {
            var theaterInDb = _db.Theaters.Include(i => i.Seats).Where(i => i.Id == id).Select(i => new TheaterVm
            {
                Id = i.Id,
                Name = i.Name,
                TotalSeats = i.Seats.Count,
                CreatedAt = i.CreatedAt
            }).FirstOrDefault();

            if (theaterInDb == null)
            {
                throw new Exception("影廳不存在!");
            }

            return theaterInDb;
        }
    }
}
