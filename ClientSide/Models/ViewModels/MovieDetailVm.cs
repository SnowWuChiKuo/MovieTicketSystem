using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClientSide.Models.ViewModels
{
	public class MovieDetailVm
	{
		public int Id { get; set; }

		public int GenreId { get; set; }

		public string GenreName { get; set; }

		public int RatingId { get; set; }

		public string RatingName { get; set; }

		[Required]
		[StringLength(70)]
		public string Title { get; set; }

		[Required]
		[StringLength(2000)]
		public string Description { get; set; }

		[Required]
		[StringLength(100)]
		public string Director { get; set; }

		[Required]
		[StringLength(1000)]
		public string Cast { get; set; }

		public int? RunTime { get; set; }

		[JsonConverter(typeof(IsoDateTimeConverter))]
		public DateTime ReleaseDate { get; set; }

		[StringLength(70)]
		public string PosterURL { get; set; }

		[DisplayFormat(DataFormatString = "{0:F1}")]
		public double AverageRating { get; set; } // 平均評分 小數點一位
		public int ReviewCount { get; set; } // 評論數量

		public bool IsLoggedIn { get; set; }
		public string CurrentUserName { get; set; }
		public bool CanReview { get; set; }

		//評論區資料
		public List<ReviewDisplayVm> Reviews { get; set; }

	}



}