using Contingency.Models;
using DungeonLib.Model;
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
        [HttpGet]
        public ActionResult Create()
        {
            DungeonCreator dc = new DungeonCreator();
            return View(dc);
        }
        [HttpPost]
        public ActionResult Create(DungeonCreator dc)
        {
            return RedirectToAction("DM", dc);
        }
        public ActionResult DM(DungeonCreator dc)
        {
            return View(dc);
        }
        
    }
}