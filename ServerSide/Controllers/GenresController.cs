using Microsoft.AspNetCore.Mvc;
using ServerSide.Models.DTOs;
using ServerSide.Models.EFModels;
using ServerSide.Models.Interfaces;
using ServerSide.Models.Services;
using ServerSide.Models.ViewModels;

namespace ServerSide.Controllers
{
	public class GenresController : Controller
	{
		private readonly IGenreService _service;

		public GenresController(IGenreService service)
		{
			_service = service;
		}

		[HttpGet]
		public IActionResult Search(string name)
		{
			var indexData = _service.GetAll();
			if (!string.IsNullOrEmpty(name)) indexData = indexData.Where(i => i.Name.Contains(name,StringComparison.OrdinalIgnoreCase)).ToList();
			return View("Index", indexData);
		}

		[HttpGet]
		public IActionResult Index()
		{
			var indexData = _service.GetAll();
			return View(indexData);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(GenreVm model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			GenreDto dto = ConvertToDto(model);
			_service.Create(dto);
			return RedirectToAction("Index");
		}

		[HttpGet]
		public IActionResult Edit(int? id)
		{
			if (id == null)
			{
				return BadRequest("ID 不能為空。");
			}
			var genreDto = _service.FindGenreById(id.Value);
			if (genreDto == null)
			{
				return NotFound("找不到指定分類");
			}
			var genreVm = ConvertToVm(genreDto);
			return View(genreVm);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(GenreVm model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			GenreDto genreDto = ConvertToDto(model);
			try
			{
				_service.Edit(genreDto);
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("",ex.Message);
				return View(model);
			}
			return RedirectToAction("Index");
		}


		private GenreVm ConvertToVm(GenreDto genreDto)
		{
			return new GenreVm
			{
				Id = genreDto.Id,
				Name = genreDto.Name,
				DisplayOrder = genreDto.DisplayOrder
			};
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Delete(int id)
		{
			try
			{
				_service.Delete(id);
				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("",ex.Message);
				return View("Edit", new GenreVm{ Id = id });
			}
		}

		private GenreDto ConvertToDto(GenreVm model)
		{
			return new GenreDto
			{
				Id = model.Id,
				Name = model.Name,
				DisplayOrder = model.DisplayOrder
			};
		}
	}
}
