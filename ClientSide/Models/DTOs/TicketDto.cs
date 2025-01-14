using ClientSide.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientSide.Models.DTOs
{
	public class TicketDto
	{
		public Movie Movie { get; set; }

		public Theater Theater { get; set; }

		public DateTime Televising { get; set; } // 播放日

		public TimeSpan StartTime { get; set; } // 播放時間

		public string SalesType { get; set; }

		public string TicketType { get; set; }
		public int ReservedSeats { get; set; }

		public int Price { get; set; }

		public string Row { get; set; }  // 行

		public int Number { get; set; }  // 列

		public bool IsDisabled { get; set; }  // 是否是殘障座位

		public string Status { get; set; }   // 座位狀態

	}
}