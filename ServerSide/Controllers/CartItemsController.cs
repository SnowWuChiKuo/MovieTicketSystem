using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerSide.Models.DTOs;
using ServerSide.Models.Services;
using ServerSide.Models.ViewModels;
using System.Security.Claims;

namespace ServerSide.Controllers
{
    public class CartItemsController : Controller
    {
        private readonly CartItemService _service;
        public CartItemsController(CartItemService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            var data = _service.GetAll();
            return View(data);
        }


        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Create(CartItemCreateVm model)
        {
            if (!ModelState.IsValid) return View(model);

            CartItemDto dto = _service.ConvertToDtoForCreate(model);

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
        // 在 CartItemsController 中新增這個方法
        [HttpGet]
        [Authorize]
        public IActionResult CheckCartQuantity(int cartId, int quantityToAdd, int? ticketIdToExclude)
        {
            try
            {
                int currentTotal = _service.GetTotalQuantityInCart(cartId, ticketIdToExclude);
                bool exceedsLimit = currentTotal + quantityToAdd > 6;
                //超過 6 返回 true
                return Json(new { exceedsLimit = exceedsLimit, currentTotal = currentTotal });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return NotFound();
            }
        }
        // 這個是頁面 ajax 要找 ticketId 會需要使用到的 controller
        [HttpGet]
        [Authorize]
        public IActionResult GetTicket(int ticketId)
        {
            try
            {
                var ticket = _service.GetTicketById(ticketId);
                if (ticket == null)
                {
                    return NotFound();
                }
                var salesType = ticket.SalesType;
                var ticketType = ticket.TicketType;
                var result = new
                {
                    // 修改這裡，使用 name，讓 JavaScript 程式碼可以正確取得資料
                    name = $"{salesType} - {ticketType}",
                    price = ticket.Price
                };

                return Json(new { data = result });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return NotFound();
            }
        }

        [HttpGet]
        [Authorize]
        public IActionResult Edit(int id)
        {
            CartItemDto cartItemDto = _service.Get(id);
            CartItemEditVm model = new CartItemEditVm
            {
                Id = cartItemDto.Id,
                CartId = cartItemDto.CartId,
                TicketId = cartItemDto.TicketId,
                Qty = cartItemDto.Qty,
                TicketPrice = _service.GetTicketById(cartItemDto.TicketId).Price,
                TicketName = cartItemDto.TicketName,
                SubTotal = cartItemDto.SubTotal,
                CreatedAt = cartItemDto.CreatedAt, // 設定創建時間
                UpdatedAt = cartItemDto.UpdatedAt, // 設定更新時間
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Edit(CartItemEditVm model)
        {
            if (!ModelState.IsValid) return View(model);

            CartItemDto dto = _service.ConvertToDtoForEdit(model);

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

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCartItem(CartItemEditVm model)
        {
            //if (!ModelState.IsValid) return View(model);

            _service.Delete(model.CartId , model.TicketId);

            TempData["ShowSuccessAlert"] = true;
            TempData["SweetAlertTitle"] = "該購物車物品已刪除";
            TempData["SweetAlertText"] = "該購物車物品已成功刪除。";
            TempData["SweetAlertIcon"] = "success";

            return RedirectToAction("Index");
        }
    }
}
