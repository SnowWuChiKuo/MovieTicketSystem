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

		public TicketsController(TicketService service)
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
			var data = _service.Get(id);

			return View(data);
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
