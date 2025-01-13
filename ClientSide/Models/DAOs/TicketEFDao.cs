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

        public List<ScreeningChangeDateVm> GetShowTimes(string theaterName)
		{
			var db = new AppDbContext();
			var data = db.Screenings.Where(d => d.Theater.Name == theaterName)
									.Select(d => new ScreeningVm 
									{
										Televising = GetTelevising(theaterName),
										StartTime = GetStartTime(theaterName),
										TheaterId = d.TheaterId,
										Id = d.Id,
									}).ToList();

			// 格式化日期時間以供 JSON 使用
			var formattedShowTimes = data.Select(st => new ScreeningChangeDateVm
			{
				Televising = st.Televising.ToString("yyyy-MM-dd"), // 或其他你需要的格式
				StartTime = st.StartTime.ToString(@"hh\:mm"), // TimeSpan 格式化
				TheaterId = st.TheaterId,
				Id = st.Id,
			}).ToList();

			return formattedShowTimes;
		}

		private TimeSpan GetStartTime(string theaterName)
		{
			throw new NotImplementedException();
		}

		private DateTime GetTelevising(string theaterName)
		{
			throw new NotImplementedException();
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
								 }).ToList();

			if (data == null) throw new Exception("找不到票種");
			return data;
		}

		public List<SeatStatusVm> GetSeatStatus(int screeningId)
		{
			var db = new AppDbContext();

			var data = db.SeatStatus.Where(d => d.ScreeningId == screeningId)
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