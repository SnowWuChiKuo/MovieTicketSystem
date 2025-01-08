using Microsoft.AspNetCore.Mvc;
using ServerSide.Models.Interfaces;

namespace ServerSide.Controllers
{
    public class ScreeningsController : Controller
    {
        private readonly IScreeningService _service;
        public ScreeningsController(IScreeningService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var indexData = _service.GetScreeningList();
            return View(indexData);
        }


		[HttpGet]
		public IActionResult Edit(int? id)
		{
			if (id == null)
			{
				return BadRequest("資料已被更動或被刪除");
			}

			var editVm = _service.GetEditList(id.Value);

			if (editVm == null)
			{
				return NotFound("找不到資料");
			}

			return View(editVm);
		}
	}
}
