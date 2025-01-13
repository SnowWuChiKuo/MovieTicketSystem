using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ClientSide.Models.ViewModels;

namespace ClientSide.Models.DTOs
{
	public class MovieDetailDto
	{
		public int Id { get; set; }

		public int GenreId { get; set; }

		public string GenreName { get; set; }

		public int RatingId { get; set; }

		public string RatingName { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public string Director { get; set; }

		public string Cast { get; set; }

		public int? RunTime { get; set; }

		[JsonConverter(typeof(IsoDateTimeConverter))]
		public DateTime ReleaseDate { get; set; }

		public string PosterURL { get; set; }

		public int ReviewCount { get; set; }
		public double? AverageRating { get; set; }


		public bool IsLoggedIn { get; set; }
		public string CurrentUserName { get; set; }
		public bool CanReview { get; set; }

		//評論區資料
		public List<ReviewDisplayVm> Reviews { get; set; }
	}



}