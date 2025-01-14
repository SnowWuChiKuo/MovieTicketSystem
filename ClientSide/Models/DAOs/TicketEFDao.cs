using ClientSide.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ClientSide.Models.ViewModels;
using System.Web.UI.WebControls;

namespace ClientSide.Models.DAOs
{
	public class TicketEFDao
	{
		public List<TheaterVm> GetTheaters(int movieId)
		{
            var db = new AppDbContext();
			var data = db.Theaters.Select(d => new TheaterVm
										{
											Id = d.Id,
											Name = d.Name,
										}).ToList();
			return data;
        }

        public List<ScreeningChangeDateVm> GetShowTimes(string theaterName, int movieId)
        {
            var db = new AppDbContext();
            var currentDateTime = DateTime.Now;

            // 設定今天的起始時間和結束時間
            var todayStart = currentDateTime.Date;
            var todayEnd = todayStart.AddDays(1);

            var data = db.Screenings
                .Where(d => d.Theater.Name == theaterName)
				.Where(d => d.MovieId == movieId)
                .Where(d =>
                    // 如果是今天的場次
                    (d.Televising >= todayStart && d.Televising < todayEnd && d.StartTime > currentDateTime.TimeOfDay) ||
                    // 如果是未來的場次
                    d.Televising >= todayEnd
                )
                // 加入排序：先按日期排，再按時間排
                .OrderBy(d => d.Televising)
                .ThenBy(d => d.StartTime)
                .Select(d => new ScreeningVm
                {
                    Televising = d.Televising,
                    StartTime = d.StartTime,
                    TheaterId = d.TheaterId,
                    Id = d.Id,
                })
                .ToList();

            // 格式化日期時間
            var formattedShowTimes = data
                .Select(st => new ScreeningChangeDateVm
                {
                    Televising = st.Televising.ToString("yyyy-MM-dd"),
                    StartTime = st.StartTime.ToString(@"hh\:mm"),
                    TheaterId = st.TheaterId,
                    Id = st.Id,
                })
                .ToList();

            return formattedShowTimes;
        }


        public List<TicketVm> GetTicket(int screeningId)
		{
			var db = new AppDbContext();
			
			var data = db.Tickets.Where(d => d.ScreeningId == screeningId)
								 .Select(d => new TicketVm
								 {
									 Id = d.Id,
									 ScreeningId = d.ScreeningId,
									 SalesType = d.SalesType,
									 TicketType = d.TicketType,
									 Price = d.Price,
									 ReservedSeats = d.ReservedSeats,
								 }).ToList();

			if (data == null) throw new Exception("找不到票種");
			return data;
		}

		public List<SeatStatusVm> GetSeatStatus(int screeningId, string theaterName)
		{
			var db = new AppDbContext();

			var data = db.SeatStatus.Where(d => d.ScreeningId == screeningId)
									.Where(d => d.Screening.Theater.Name == theaterName)
									.Include(d => d.Seat)
									.Select(d => new SeatStatusVm
									{
										Id = d.Id,
										ScreeningId = d.ScreeningId,
										SeatId = d.SeatId,
										Status = d.Status,
										Row = d.Seat.Row,         // 直接使用座位的 Row 值
										Number = d.Seat.Number,   // 直接使用座位的 Number 值
										IsDisabled = d.Seat.IsDisabled,
									}).ToList();
			if (data == null) throw new Exception("找不到此場次做位");
			return data;
		}


    }
}