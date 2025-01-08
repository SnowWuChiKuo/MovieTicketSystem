using Microsoft.AspNetCore.Mvc;
using ServerSide.Models.Services;

namespace ServerSide.Controllers
{
    public class OrderItemsController : Controller
    {
        private readonly OrderItemService _service;

        public OrderItemsController(OrderItemService service)
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

        // 這個是前端要找 ticketId 會需要使用到的 controller
        [HttpGet]
        public IActionResult GetTicketName(int ticketId)
        {
            var ticket = _service.GetTicketNameById(ticketId);
            if (ticket != null)
            {
                return Json(new { data = ticket.TicketType });
            }
            return NotFound();
        }
    }
}
