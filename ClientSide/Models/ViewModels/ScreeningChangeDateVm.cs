using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientSide.Models.ViewModels
{
	public class ScreeningChangeDateVm
	{
		public int Id { get; set; }
		public int TheaterId { get; set; }
		public string Televising { get; set; } // 播放日

		public string StartTime { get; set; } // 播放時間
	}
}