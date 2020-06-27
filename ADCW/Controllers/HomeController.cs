using ADCW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ADCW.Controllers
{
    [Authorize(Roles = "Assistant, Manager")]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            ViewBag.LastName = new SelectList(db.Actors.ToList(), "ActorLastName", "ActorLastName");
            ViewBag.MemberLN = new SelectList(db.Members.ToList(), "MemberLastName", "MemberLastName");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}