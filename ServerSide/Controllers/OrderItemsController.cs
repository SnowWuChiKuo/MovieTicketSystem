using Humanizer;
using Microsoft.AspNetCore.Mvc;
using ServerSide.Models.DTOs;
using ServerSide.Models.Services;
using ServerSide.Models.ViewModels;

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
            if (ticket == null)
            {
                return NotFound();
            }

            var salesType = ticket.SalesType;
            var ticketType = ticket.TicketType;

            var result = new
            {
                ticketName = $"{salesType} - {ticketType}",
                price = ticket.Price
            };

            return Json(new { data = result });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(OrderItemVm model)
        {
            if (!ModelState.IsValid) return View(model);

            OrderItemDto dto = ConvertToDTO(model);

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

        private OrderItemDto ConvertToDTO(OrderItemVm model)
        {
            return new OrderItemDto
            {
                Id = model.Id,
                OrderId = model.OrderId,
                TicketId = model.TicketId,
                TicketName = model.TicketName,
                Price = model.Price,
                Qty = model.Qty,
                SubTotal = model.SubTotal,
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
        public IActionResult Edit(OrderItemVm model)
        {
            if(!ModelState.IsValid) return View(model);

            OrderItemDto dto = ConvertToDTO(model);

            try
            {
                _service.Edit(dto);
                return RedirectToAction("Index");
            }
            catch(Exception ex) 
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
