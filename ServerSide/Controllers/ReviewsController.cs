﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerSide.Models.DTOs;
using ServerSide.Models.Interfaces;
using ServerSide.Models.ViewModels;

namespace ServerSide.Controllers
{
    [Authorize]
    public class ReviewsController : Controller
    {
        private readonly IReviewService _service;
        public ReviewsController(IReviewService service)
        {
            _service = service;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var indexData = _service.GetAllReview();
            return View(indexData);
        }

        [HttpGet]
        public IActionResult Search(string Account,string Title,string Keyword, DateTime? StartDate, DateTime? EndDate)
        {
            var indexData = _service.GetAllReview();
            if(!string.IsNullOrEmpty(Account))
                indexData = indexData.Where(i => i.MemberAccount == Account).ToList();

            if(!string.IsNullOrEmpty(Title))
                indexData = indexData.Where(i => i.MovieTitle.Contains(Title, StringComparison.OrdinalIgnoreCase)).ToList();

            if(!string.IsNullOrEmpty(Keyword))
                indexData = indexData
                    .Where(i => !string.IsNullOrEmpty(i.Comment) && i.Comment.Contains(Keyword, StringComparison.OrdinalIgnoreCase)).ToList();

            if (StartDate.HasValue)
                indexData = indexData.Where(i => i.CreatedAt >= StartDate.Value).ToList();

            if (EndDate.HasValue)
                indexData = indexData.Where(i => i.CreatedAt <= EndDate.Value).ToList();

            return View("Index", indexData);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ReviewVm vm)
        {
            if(!ModelState.IsValid) return View(vm);
            ReviewDto dto = ConvertToDto(vm);

            try
            {
                _service.Create(dto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(vm);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id==null) return BadRequest("請求錯誤，資料Id不能為空，請檢查資料是否存在。");
            var dto = _service.FindReviewById(id.Value);
            if(dto == null) return NotFound("該筆評論不存在或已被刪除");
            var vm = ConvertToVm(dto);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ReviewVm vm)
        {
            if(!ModelState.IsValid) return View(vm);
            ReviewDto dto = ConvertToDto(vm);

            try
            {
            _service.Edit(dto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(vm);
            }

            return RedirectToAction("Index");
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
                ModelState.AddModelError("", ex.Message);
				return View("Edit", new ReviewVm{Id = id});
			}

		}

		private ReviewVm ConvertToVm(ReviewDto dto)
        {
            return new ReviewVm
            {
                Id = dto.Id,
                MovieId = dto.MovieId,
                MovieTitle = dto.MovieTitle,
                MemberId = dto.MemberId,
				MemberAccount = dto.MemberAccount,
                OrderId = dto.OrderId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = dto.CreatedAt,
                UpdatedAt = dto.UpdatedAt,
            };
        }

        private ReviewDto ConvertToDto(ReviewVm vm)
        {
            return new ReviewDto
            {
                Id = vm.Id,
                MovieId = vm.MovieId,
                MemberId = vm.MemberId,
                OrderId = vm.OrderId,
                Rating = vm.Rating,
                Comment = vm.Comment,
                CreatedAt = vm.CreatedAt,
                UpdatedAt = vm.UpdatedAt,
            };
        }
    }
}
