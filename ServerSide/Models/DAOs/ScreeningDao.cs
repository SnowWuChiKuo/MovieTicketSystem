using Microsoft.EntityFrameworkCore;
using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.Interfaces;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.DAOs
{
    public class ScreeningDao : IScreeningDao
    {
        private readonly AppDbContext _db;

        public ScreeningDao(AppDbContext db)
        {
            _db = db;
        }


        public void Create(ScreeningDto dto)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public ScreeningDto GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ScreeningVm> GetScreeningList()
        {
            var indexData = _db.Screenings
                .Include(s => s.Movie)
                .Include(s => s.Theater)
                .Select(s => new ScreeningVm
                {
                    Id = s.Id,
                    MovieId = s.MovieId,
                    MovieTitle = s.Movie.Title,
                    TheaterId = s.TheaterId,
                    ScreeningDate = s.Movie.ReleaseDate,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    CreatedAt = s.CreatedAt
                }).ToList();
            return indexData;
        }

        public bool IsScreeningExist(int id)
        {
            throw new NotImplementedException();
        }


        public void Update(ScreeningDto dto)
        {
            throw new NotImplementedException();
        }


        public ScreeningDto ConvertToDto(Screening screening)
        {
            throw new NotImplementedException();
        }

        public Screening ConvertToEfEntity(ScreeningDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
