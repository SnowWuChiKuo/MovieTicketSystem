using ClientSide.Models.DTOs;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClientSide.Models.ViewModels
{
	public class MovieIndexVm
	{
		public int Id { get; set; }
		public string Title { get; set; }

        [JsonConverter(typeof(IsoDateTimeConverter))]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime ReleaseDate { get; set; }

		[StringLength(70)]
		public string PosterURL { get; set; }

		public int ReviewCount { get; set; }

	}

}