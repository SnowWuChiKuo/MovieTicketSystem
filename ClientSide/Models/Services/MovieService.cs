using ClientSide.Models.DTOs;
using ClientSide.Models.Repository;
using ClientSide.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClientSide.Models.Services
{
	public class MovieService
	{
		private readonly MovieRepository _repo;
		public MovieService()
		{
			_repo = new MovieRepository();
		}
		public MovieDetailDto GetMovieById(int id)
		{
			return _repo.GetMovieById(id);
		}
		public List<MovieIndexDto> GetIndexMovie()
		{
			return _repo.GetIndexMovie();
		}

		public List<MovieIndexDto> GetLatestMovies()
		{
			return _repo.GetLatestMovies();
		}

		public List<MovieIndexDto> GetMostReviewedMovies()
		{
			return _repo.GetMostReviewedMovies();
		}

		public List<MovieIndexDto> GetUpcomingMovies()
		{
			return _repo.GetUpcomingMovies();
		}
		public bool CheckUserHasValidOrder(string acc, int movieId)
		{
			return _repo.CheckUserHasValidOrder(acc,movieId);
		}

		public ReviewDisplayVm AddReview(string account,ReviewCreateDto dto)
		{
			return _repo.AddReview(account,dto);
		}
	}
}