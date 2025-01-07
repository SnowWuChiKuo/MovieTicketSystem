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
		private readonly TicketSeatService _service;

		public TicketSeatsController(TicketSeatService service)
		{
			_service = service;
		}

		[HttpGet]
		public IActionResult Index()
		{
			var data = _service.GetAll();

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

			TicketSeatDto dto = ConvertToDTO(model);

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

		private TicketSeatDto ConvertToDTO(TicketSeatVm model)
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
			var data = _service.Get(id);

			return View(data);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(TicketSeatVm model)
		{
			if (!ModelState.IsValid) return View(model);

			TicketSeatDto dto = ConvertToDTO(model);

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
