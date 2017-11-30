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
        DungeonCreator currentDungeonToView;
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
            currentDungeonToView = dc;
            return RedirectToAction("Dungeon");
        }
        public ActionResult Dungeon()
        {
            return View(currentDungeonToView);
        }
        public ActionResult Room(int dungeonID, int roomID)
        {
            //set currentDungeonToView to the dungeon with dungeonid, and change the model to be a room object
            return View(currentDungeonToView);
        }
        public ActionResult Configure(int dungeonID)
        {
            //set currentDungeonToView to the dungeon with dungeonid
            return View(currentDungeonToView);
        }
        public ActionResult Players(int dungeonID)
        {
            //set currentDungeonToView to the dungeon with dungeonid
            return View(currentDungeonToView);
        }
    }
}