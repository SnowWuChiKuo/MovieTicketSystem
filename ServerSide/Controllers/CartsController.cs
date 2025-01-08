using Microsoft.AspNetCore.Mvc;
using ServerSide.Models.Services;

namespace ServerSide.Controllers
{
    public class CartsController : Controller
    {
        private readonly CartService _service;

        public CartsController(CartService service)
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
            var data = _service.GetMembersAccount();
            ViewBag.MemberName = data;
            return View();
        }

        
    }
}
