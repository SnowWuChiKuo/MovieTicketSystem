using ClientSide.Models.DTOs;
using ClientSide.Models.Services;
using ClientSide.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientSide.Controllers
{
    public class MembersController : Controller
    {
        // GET: Members
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterVm model)
        {
            if (!ModelState.IsValid) return View(model);
            try
            {
                ProcessRegister(model);
                return View("RegisterConfirm");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("",ex.Message);
                return View(model);
            }

        }

        private void ProcessRegister(RegisterVm model)
        {
            RegisterDto dto = new RegisterDto
            {
                Name = model.Name,
                Account = model.Account,
                Password = model.Password,
                Email = model.Email,
            };
            new MemberService().Register(dto);
        }
    }
}