using Microsoft.AspNetCore.Mvc;
using ServerSide.Models.DTOs;
using ServerSide.Models.Services;
using ServerSide.Models.ViewModels;

namespace ServerSide.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserService _service;
        public UsersController(UserService service)
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
        public IActionResult Create(MemberCreateVm model)
        {
            if (!ModelState.IsValid) return View(model);

            MemberDto dto = _service.ConvertToDto(model);
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

        [HttpGet]
        public IActionResult Edit(int id)
        {
            MemberDto memberDto = _service.Get(id);
            MemberCreateVm model = new MemberCreateVm
            {
                Account = memberDto.Account,
                Email = memberDto.Email,
                Name = memberDto.Name,
                IsDeleted = memberDto.IsDeleted,
                IsConfirmed = memberDto.IsConfirmed,
                IsBlackList = memberDto.IsBlackList,

            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(MemberCreateVm model)
        {
            if (!ModelState.IsValid) return View(model);

            MemberDto dto = _service.ConvertToDto(model);
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

    }
}
