using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientSide.Models.ViewModels
{
	//傳遞回資料庫的評論資料
	public class ReviewCreateDto
	{
		public int MovieId { get; set; }
		public string Account { get; set; }
		public int Rating { get; set; }
		public string Comment { get; set; }
		public int OrderId { get; set; }

	}
}