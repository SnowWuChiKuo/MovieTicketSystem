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
    }
}
