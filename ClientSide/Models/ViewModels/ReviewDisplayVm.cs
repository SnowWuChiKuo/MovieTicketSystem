using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [JsonConverter(typeof(IsoDateTimeConverter))]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedAt { get; set; }

		public int Rating { get; set; }
		public int OrderId { get; set; }
	}
}