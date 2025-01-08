using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ServerSide.Models.DTOs;
using ServerSide.Models.Infra;
using ServerSide.Models.Services;
using ServerSide.Models.ViewModels;

namespace ServerSide.Controllers
{
    public class MembersController : Controller
    {
        private readonly MemberService _service;
        public MembersController(MemberService service)
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
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
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
