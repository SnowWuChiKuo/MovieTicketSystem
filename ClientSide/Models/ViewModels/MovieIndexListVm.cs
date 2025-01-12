using ClientSide.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientSide.Models.ViewModels
{
	/// <summary>
	/// 首頁輪播、MovieCard區的ViewModel
	/// </summary>
	public class MovieIndexListVm
	{
		public List<MovieIndexVm> LatestMovies { get; set; }
		public List<MovieIndexVm> MostReviewedMovies { get; set; }
		public List<MovieIndexVm> UpcomingMovies { get; set; }
	}
}