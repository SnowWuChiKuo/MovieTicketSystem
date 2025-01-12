﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClientSide.Models.ViewModels
{
	public class SeatStatusVm
	{
		public int Id { get; set; }

		public int ScreeningId { get; set; }

		public int SeatId { get; set; }

		public string Status { get; set; }

		public int Row { get; set; }

		public int Number { get; set; }

		public bool IsDisabled { get; set; }
	}
}