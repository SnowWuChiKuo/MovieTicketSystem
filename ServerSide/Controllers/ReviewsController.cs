using Microsoft.AspNetCore.Mvc;
using ServerSide.Models.DTOs;
using ServerSide.Models.Interfaces;
using ServerSide.Models.ViewModels;

namespace ServerSide.Controllers
{
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
        public IActionResult Search(string name,string title,string keyword, DateTime? startDate, DateTime? endDate)
        {
            var indexData = _service.GetAllReview();
            if(!string.IsNullOrEmpty(name))
                indexData = indexData.Where(i => i.MemberName == name).ToList();

            if(!string.IsNullOrEmpty(title))
                indexData = indexData.Where(i => i.MovieTitle.Contains(title,StringComparison.OrdinalIgnoreCase)).ToList();

            if(!string.IsNullOrEmpty(keyword))
                indexData = indexData
                    .Where(i => !string.IsNullOrEmpty(i.Comment) && i.Comment.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();

            if (startDate.HasValue)
                indexData = indexData.Where(i => i.CreatedAt >= startDate.Value).ToList();

            if (endDate.HasValue)
                indexData = indexData.Where(i => i.CreatedAt <= endDate.Value).ToList();

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

        private ReviewVm ConvertToVm(ReviewDto dto)
        {
            return new ReviewVm
            {
                Id = dto.Id,
                MovieId = dto.MovieId,
                MovieTitle = dto.MovieTitle,
                MemberId = dto.MemberId,
                MemberName = dto.MemberName,
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
