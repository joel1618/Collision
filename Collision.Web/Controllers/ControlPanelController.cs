using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Collision.Controllers
{
    public class ControlPanelController : Controller
    {
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                //return View();
                return View("Index", "~/Views/Shared/_OriginalLayout.cshtml");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
