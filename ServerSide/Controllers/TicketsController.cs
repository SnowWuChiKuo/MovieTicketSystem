using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.Services;
using ServerSide.Models.ViewModels;

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
				ScreeningId = model.ScreeningId,
				SalesType = model.SalesType,
				TicketType = model.TicketType,
				Price = model.Price
			};
		}
	}
}
