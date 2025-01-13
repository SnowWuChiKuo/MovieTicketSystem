using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientSide.Models.ViewModels
{
	public class ScreeningVm
	{
		public int Id { get; set; }
		public int TheaterId { get; set; }
		public DateTime Televising { get; set; } // 播放日

		public TimeSpan StartTime { get; set; } // 播放時間

	}
}