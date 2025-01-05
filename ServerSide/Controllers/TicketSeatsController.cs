using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.Services;
using ServerSide.Models.ViewModels;

namespace ServerSide.Controllers
{
	public class TicketSeatsController : Controller
	{
		private readonly AppDbContext _db;

		private readonly TicketSeatService _service;

		public TicketSeatsController(AppDbContext db, TicketSeatService service)
		{
			_db = db;
			_service = service;
		}

		[HttpGet]
		public IActionResult Index()
		{
			var data = _db.TicketSeats.Include(d => d.Seat)
										.Include(d => d.Ticket)
										.Select(d => new TicketSeatVm
										{
											Id = d.Id,
											SeatId = d.SeatId,
											TicketId = d.TicketId,
										}).ToList();

			return View(data);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(TicketSeatVm model)
		{
			if (!ModelState.IsValid) return View(model);

			TicketSeatDto dto = ConverToDTO(model);

			try
			{
				_service.Create(dto);
				return RedirectToAction("Index");
			}
			catch (Exception ex) 
			{
				ModelState.AddModelError("", ex.Message);
				return View(model);
			}

		}

		private TicketSeatDto ConverToDTO(TicketSeatVm model)
		{
			return new TicketSeatDto{
				Id = model.Id,
				SeatId = model.SeatId,
				TicketId = model.TicketId,
			};
		}

		[HttpGet]
		public IActionResult Edit(int id)
		{ 
			var ticketSeatInDb = _db.TicketSeats.AsNoTracking()
												.Include(d => d.Ticket)
												.Include(d => d.Seat)
												.FirstOrDefault(d => d.Id == id);
			if (ticketSeatInDb == null) return NotFound();

			var model = new TicketSeatVm
			{
				Id = ticketSeatInDb.Id,
				TicketId = ticketSeatInDb.TicketId,
				SeatId = ticketSeatInDb.SeatId,
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(TicketSeatVm model)
		{
			if (!ModelState.IsValid) return View(model);

			TicketSeatDto dto = ConverToDTO(model);

			try
			{
				_service.Edit(dto);
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", ex.Message);
				return View(model);
			}
		}

		[HttpGet]
		public IActionResult Delete(int? id) 
		{
			if (id == null) return NotFound();

			try
			{
				_service.Delete(id.Value);
				return RedirectToAction("Index");
			}
			catch (Exception ex) 
			{
				ViewBag.ErrorMessage = ex.Message;
				return View();
			}
		}
	}
}
