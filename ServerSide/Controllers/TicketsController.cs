using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.Services;
using ServerSide.Models.ViewModels;
using System.Net;

namespace ServerSide.Controllers
{
	public class TicketsController : Controller
	{
		private readonly TicketService _service;
		private readonly AppDbContext _db;

		public TicketsController(TicketService service, AppDbContext db)
		{
			_service = service;
			_db = db;
		}

		[HttpGet]
		public IActionResult Index()
		{
			var data = _db.Tickets.Include(d => d.Screening)
								.Select(d => new TicketVm
								{
									Id = d.Id,
									ScreeningId = d.ScreeningId,
									SalesType = d.SalesType,
									TicketType = d.TicketType,
									Price = d.Price
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
		public IActionResult Create(TicketVm model)
		{
			if (!ModelState.IsValid) return View(model);

			TicketDto dto = ConverToDTO(model);

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

		private TicketDto ConverToDTO(TicketVm model)
		{
			 return new TicketDto
			{
				 Id = model.Id,
				ScreeningId = model.ScreeningId,
				SalesType = model.SalesType,
				TicketType = model.TicketType,
				Price = model.Price
			};
		}

		[HttpGet]
		public IActionResult Edit(int id)
		{
			var ticketInDb = _db.Tickets.AsNoTracking()
										.Include(t => t.Screening)
										 .FirstOrDefault(t => t.Id == id);
			if (ticketInDb == null)
			{
				return NotFound();
			}

			var model = new TicketVm
			{
				Id = ticketInDb.Id,
				ScreeningId = ticketInDb.ScreeningId,
				SalesType = ticketInDb.SalesType,
				TicketType = ticketInDb.TicketType,
				Price = ticketInDb.Price
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(TicketVm model)
		{
			if (!ModelState.IsValid) return View(model);

			TicketDto dto = ConverToDTO(model);

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
			if (id == null) return BadRequest("找不到此Id");

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
