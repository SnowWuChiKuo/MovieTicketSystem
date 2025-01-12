using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientSide.Models.ViewModels
{
	//網頁上每一則評論的ViewModel
	public class ReviewDisplayVm
	{
		public int Id { get; set; }
		public string MemberName { get; set; }
		public string Comment { get; set; }
		public DateTime CreatedAt { get; set; }
		public int Rating { get; set; }
		public int OrderId { get; set; }
	}
}