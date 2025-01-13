using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClientSide.Models.DTOs
{
	public class AllMoviesDto
	{
		public int Id { get; set; }
		public string Title { get; set; }

		public DateTime ReleaseDate { get; set; }

		[StringLength(70)]
		public string PosterURL { get; set; }

	}
}