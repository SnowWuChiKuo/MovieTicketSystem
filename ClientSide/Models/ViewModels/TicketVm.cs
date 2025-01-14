using ClientSide.Models.EFModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ClientSide.Models.ViewModels
{
    public class TicketVm
    {
		public int Id { get; set; }

		public int ScreeningId { get; set; }

		public string SalesType { get; set; }

		public string TicketType { get; set; }
		public int ReservedSeats { get; set; }

		public int Price { get; set; }
	}
}