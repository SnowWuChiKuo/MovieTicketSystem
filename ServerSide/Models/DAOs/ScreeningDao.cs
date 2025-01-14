using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.Interfaces;
using ServerSide.Models.ViewModels;
using System.Collections.Generic;

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
            var entity = ConvertToEfEntity(dto);
            _db.Screenings.Add(entity);
            _db.SaveChanges();

            // 2. 獲取影廳所有座位
            var seats = _db.Seats
                .AsNoTracking()  // 使用 AsNoTracking 提升性能
                .Where(s => s.TheaterId == dto.TheaterId && !s.IsDisabled)  // 只取未停用的座位
                .Select(s => new  // 明確選擇需要的欄位
                {
                    s.Id,
                    s.TheaterId
                })
                .ToList();

            // 3. 為每個座位創建 SeatStatus
            var seatStatuses = seats.Select(seat => new SeatStatus
            {
                ScreeningId = entity.Id,
                SeatId = seat.Id,
                Status = "可使用", // 預設狀態為可選
                UpdatedAt = DateTime.Now
            }).ToList();

            _db.SeatStatuses.AddRange(seatStatuses);
            _db.SaveChanges();



            // 每個場次票種也不一樣
            var price = _db.Prices.Where(p => p.MovieId == dto.MovieId).ToList();
            var tickets = new List<Ticket>();

            TimeOnly earlyTime = new TimeOnly(12, 0, 0);
            TimeOnly lastTime = new TimeOnly(22, 0, 0);

            foreach (var item in price)
            {
                if ( entity.StartTime < earlyTime && item.SalesType == "早鳥優惠")
                {
                    var ticket = new Ticket
                    {
                        ScreeningId = entity.Id,
                        SalesType = item.SalesType,
                        TicketType = item.TicketType,
                        Price = item.Price1,
                        ReservedSeats = item.ReservedSeats,
                    };
                    tickets.Add(ticket);
                }

                if (entity.StartTime < lastTime && item.SalesType == "大夜票")
                {
                    var ticket = new Ticket
                    {
                        ScreeningId = entity.Id,
                        SalesType = item.SalesType,
                        TicketType = item.TicketType,
                        Price = item.Price1,
                        ReservedSeats = item.ReservedSeats,
                    };
                    tickets.Add(ticket);
                }

                if (item.SalesType == "平日票")
                {
                    var ticket = new Ticket
                    {
                        ScreeningId = entity.Id,
                        SalesType = item.SalesType,
                        TicketType = item.TicketType,
                        Price = item.Price1,
                        ReservedSeats = item.ReservedSeats,
                    };
                    tickets.Add(ticket);
                }
            }

            _db.Tickets.AddRange(tickets);
            _db.SaveChanges();
        }

        public void Edit(ScreeningDto dto)
        {
            var screeningInDb = _db.Screenings.Find(dto.Id);
            if (screeningInDb == null)
            {
                throw new Exception("找不到資料");
            }

            screeningInDb.MovieId = dto.MovieId;
            screeningInDb.TheaterId = dto.TheaterId;
            screeningInDb.Televising = dto.Televising;
            screeningInDb.StartTime = dto.StartTime;
            screeningInDb.EndTime = dto.EndTime;
            screeningInDb.UpdatedAt = dto.UpdatedAt;

            _db.SaveChanges();
        }

        /// <summary>
        /// 取得電影片長，供Create頁的JQuery去Fetch後動態抓取片長
        /// </summary>
        /// <param name="movieId"></param>
        /// <returns></returns>
        public int? GetMovieRunTime(int movieId)
        {
            return _db.Movies
                .Where(m => m.Id == movieId)
                .Select(m => m.RunTime)
                .FirstOrDefault();
        }


        public DateTime GetMovieReleaseDate(int movieId)
        {
            return _db.Movies
                .Where(m => m.Id == movieId)
                .Select(m => m.ReleaseDate.Date)
                .FirstOrDefault();
        }


        public bool IsValidTelevisingDate(int movieId, DateOnly televising)
        {
            var releaseDate =  GetMovieReleaseDate(movieId);
            return televising.ToDateTime(TimeOnly.MinValue) >= releaseDate;
        }

        /// <summary>
        /// 檢查新增或編輯場次時是否與當前既有場次時間衝突。
        /// </summary>
        /// <param name="theaterId">要新增場次的影廳</param>
        /// <param name="date"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="excludeId"> 要排除的Id(當前資料Id)，避免編輯時把自己算進去導致一定是false </param>
        /// <returns></returns>
        public bool HasTimeConflict(int theaterId, DateOnly televising, TimeOnly startTime, TimeOnly endTime, int excludeId = 0)
        {
            //先獲取當天的所有場次
            var currentScreenings = 
                _db.Screenings
                .Include(s => s.Movie)
                .Where(s => 
                s.TheaterId == theaterId && 
                s.Televising == televising &&
                s.Id != excludeId)
                .ToList();

            //檢查既有場次是否與新場次時間重疊
            foreach (var screening in currentScreenings)
            {
                //用片長計算結束時間
                var currentEndTime = screening.StartTime.AddMinutes(screening.Movie.RunTime ?? 0);

                //場次衝突的三種情況，
                // [1] 新場次開始時間在既有場次之間
                // [2] 新場次結束時間在既有場次之間
                // [3] 新場次包含既有場次

                bool startInBetween = startTime >= screening.StartTime && startTime < currentEndTime;

                bool endInBetween = endTime > screening.StartTime && endTime <= currentEndTime;

                bool contain = startTime <= screening.StartTime && endTime >= currentEndTime;

                if (startInBetween || endInBetween || contain)
                {
                    return true;
                }
            }

            return false;
        }

        public void Delete(int id)
        {
            var screeningInDb = _db.Screenings.Find(id);
            if (screeningInDb == null)
            {
                throw new Exception("找不到場次，請確認此資料是否已被更動");
            }
            _db.Screenings.Remove(screeningInDb);
            _db.SaveChanges();
        }

        public ScreeningDto GetById(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 單獨取得電影資料，給Create頁產生選單
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetMovieOptions()
        {
            return _db.Movies
                .Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = $"{m.Id} - {m.Title}",
                }).ToList();
        }

        /// <summary>
        /// 單獨取得影廳資料，給Create頁產生選單
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetTheaterOptions()
        {
            return _db.Theaters
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Name,
                }).ToList();
        }

        /// <summary>
        /// 取得場次index頁面資料
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ScreeningVm> GetScreeningList()
        {
            var indexData = _db.Screenings
                .Include(s => s.Movie)
                .Include(s => s.Theater)
                .Where(s => !s.Movie.Title.Contains("預告"))
                .Select(s => new ScreeningVm
                {
                    Id = s.Id,
                    MovieId = s.MovieId,
                    MovieTitle = s.Movie.Title,
                    TheaterId = s.TheaterId,
                    Televising = s.Televising,
                    StartTime = s.StartTime,
                    EndTime = s.StartTime.AddMinutes(s.Movie.RunTime ?? 0),
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt
                }).ToList();
            return indexData;
        }

        /// <summary>
        /// 取得Edit頁面資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ScreeningEditVm GetEditList(int id)
        {
            var screening = _db.Screenings
                .Include(e => e.Movie)
                .Include(e => e.Theater)
                .FirstOrDefault(e => e.Id == id);

            var movieOptions = _db.Movies
                .Select(m => new SelectListItem
                {
                    Value = m.Id.ToString(),
                    Text = $"{m.Id} - {m.Title}",
                    Selected = screening != null && m.Id == screening.MovieId
                })
                .ToList();

            var theaterOptions = _db.Theaters
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Name,
                    Selected = screening != null && t.Id == screening.TheaterId
                })
                .ToList();

            if (screening == null)
            {
                return new ScreeningEditVm
                {
                    MovieOptions = movieOptions,
                    TheaterOptions = theaterOptions
                };
            }
            var editVm = new ScreeningEditVm
            {
                Id = screening.Id,
                MovieId = screening.MovieId,
                MovieTitle = screening.Movie.Title,
                TheaterId = screening.TheaterId,
                Televising = screening.Televising,
                StartTime = screening.StartTime,
                RunTime = screening.Movie.RunTime,
                EndTime = screening.StartTime.AddMinutes(screening.Movie.RunTime.HasValue ? screening.Movie.RunTime.Value : 0),
                MovieOptions = movieOptions,
                TheaterOptions = theaterOptions
            };

            return editVm;
        }

        public bool IsScreeningExist(int id)
        {
            throw new NotImplementedException();
        }



		public ScreeningDto ConvertToDto(Screening screening)
        {
            return new ScreeningDto
            {
                Id = screening.Id,
                MovieId = screening.MovieId,
                TheaterId = screening.TheaterId,
                Televising = screening.Televising,
                StartTime = screening.StartTime,
                EndTime = screening.EndTime,
                CreatedAt = screening.CreatedAt,
                UpdatedAt = screening.UpdatedAt
            };
        }

        public Screening ConvertToEfEntity(ScreeningDto dto)
        {
            return new Screening
            {
                Id = dto.Id,
                MovieId = dto.MovieId,
                TheaterId = dto.TheaterId,
                Televising = dto.Televising,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt
            };
        }


	}
}
