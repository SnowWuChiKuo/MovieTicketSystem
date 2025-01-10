using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ClientSide.Controllers
{
    public class MoviesController : Controller
    {
        // GET: Movies
        public ActionResult Main()
        {
            return View();
        }

        public ActionResult Movie(int movieId)
        {
            return View();
        }
    }
}