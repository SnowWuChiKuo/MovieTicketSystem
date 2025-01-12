using ClientSide.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using ClientSide.Models.ViewModels;

namespace ClientSide.Models.DAOs
{
	public class TicketEFDao
	{
		public List<ScreeningChangeDateVm> GetShowTimes(int theaterId)
		{
			var db = new AppDbContext();
			var data = db.Screenings.Where(d => d.TheaterId == theaterId)
									.Select(d => new ScreeningVm 
									{
										Televising = d.Televising,
										StartTime = d.StartTime,
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


		public List<TicketVm> GetTicket(int screeningId)
		{
			var db = new AppDbContext();
			
			var data = db.Tickets.Where(d => d.ScreeningId == screeningId)
								 .Select(d => new TicketVm
								 {
									 Id = d.Id,

									 SalesType = d.SalesType,
									 TicketType = d.TicketType,
									 Price = d.Price,
								 }).ToList();

			if (data == null) throw new Exception("找不到票種");
			return data;
		}

		
	}
}