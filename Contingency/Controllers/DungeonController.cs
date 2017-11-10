using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Contingency.Controllers
{
    public class DungeonController : Controller
    {
        // GET: Dungeon
        public ActionResult Create()
        {
            return View();
        }
    }
}