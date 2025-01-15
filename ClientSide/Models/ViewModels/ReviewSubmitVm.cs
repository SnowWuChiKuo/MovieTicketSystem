using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClientSide.Models.ViewModels
{

	//接收網頁評論區表單資料
	public class ReviewSubmitVm
	{
		public int MovieId { get; set; }

		[Required]
		[Range(1, 5)]
		public int Rating { get; set; }

		[Required]
		[StringLength(200)]
		public string Comment { get; set; }
	}
}