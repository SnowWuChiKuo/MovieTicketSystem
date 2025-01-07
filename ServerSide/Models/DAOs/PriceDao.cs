using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.Interfaces;
using ServerSide.Models.ViewModels;

namespace ServerSide.Models.DAOs
{
    public class PriceDao : IPriceDao
    {
        private readonly AppDbContext _db;
        public PriceDao(AppDbContext db)
        {
            _db = db;
        }
        public PriceDto ConvertToDto(Price price)
        {
            return new PriceDto
            {
                Id = price.Id,
                MovieId = price.MovieId,
                SalesType = price.SalesType,
                TicketType = price.TicketType,
                ReservedSeats = price.ReservedSeats,
                Price1 = price.Price1
            };
        }

        public Price ConvertToEFEntity(PriceDto dto)
        {
            return new Price
            {
                Id= dto.Id,
                MovieId = dto.MovieId,
                SalesType = dto.SalesType,
                TicketType = dto.TicketType,
                ReservedSeats = dto.ReservedSeats,
                Price1 = dto.Price1
            };
        }

        public void Create(PriceDto dto)
        {
            var newTix = ConvertToEFEntity(dto);
            _db.Prices.Add(newTix);
            _db.SaveChanges();
        }

        public void Delete(int id)
        {
            var priceInDb = _db.Prices.Find(id);
            if(priceInDb == null)
			{
				throw new Exception("該筆資料不存在或已被刪除!");
			};
			_db.Prices.Remove(priceInDb);
			_db.SaveChanges();
		}

        public void Edit(PriceDto dto)
        {
            var priceInDb = _db.Prices.Find(dto.Id);
            if (priceInDb == null)
            {
                throw new Exception("該筆資料不存在或已被刪除!");
            };

            priceInDb.SalesType = dto.SalesType;
            priceInDb.TicketType = dto.TicketType;
            priceInDb.ReservedSeats = dto.ReservedSeats;
            priceInDb.Price1 = dto.Price1;

            _db.SaveChanges();
        }

        public PriceDto FindPriceById(int id)
        {
            var priceInDb = _db.Prices.Find(id);
            if(priceInDb == null)
            {
                throw new Exception("該筆資料不存在或已被刪除!");
            };
            var priceDto = ConvertToDto(priceInDb);
            return priceDto;
        }

        /// <summary>
        /// 取得當前電影的所有票券資訊
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<PriceVm> GetMovieTixDetail(int id)
        {
            var tixDetail = _db.Prices
                .Where(t => t.MovieId == id)
                .Select(t => new PriceVm
                {
                    Id = t.Id,
                    MovieId = t.MovieId,
                    SalesType = t.SalesType,
                    TicketType = t.TicketType,
                    ReservedSeats = t.ReservedSeats,
                    PriceOfTicket = t.Price1
                }).ToList();
            return tixDetail;
        }
        public bool IsMovieExist(int movieId)
        {
            return _db.Movies.Any(m => m.Id == movieId);
        }
        /// <summary>
        /// 取得當前電影(方便以電影去管理票券)，並顯示各電影有提供的票券數量
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PriceIndexVm> GetMovieTixList()
        {
            var tixOfMovie = _db.Movies
                .Select(t => new PriceIndexVm
                {
                    MovieId = t.Id,
                    MovieTitle = t.Title,
                    TicketCount = _db.Prices.Count(p => p.MovieId == t.Id)
                }).ToList();

            return tixOfMovie;
        }
    }
}
