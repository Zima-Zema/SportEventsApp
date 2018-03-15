using SportEventsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using SportEventsApp.ViewModel;

namespace SportEventsApp.Controllers.Admin
{
    [Authorize]
    public class GroupsController : Controller
    {
        ApplicationDbContext _context;
        public GroupsController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Groups
        public ActionResult Index()
        {
            var groups = _context.Groups.Include(gg => gg.Event).ToList();
            return View(groups);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var group = _context.Groups.Include(uu => uu.Event).FirstOrDefault(use => use.Id == id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }
        public ActionResult New()
        {
            var events = _context.Events.ToList();

            var viewModel = new GroupsViewModel()
            {
                Events = events,
                
                Group = new Group()
            };
            return View(viewModel);
        }
        public ActionResult Save(GroupsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new GroupsViewModel()
                {
                    Events = _context.Events.ToList(),
                    Group = model.Group
                    
                };
                return View("New", viewModel);
            }
            if (model.Group.Id == 0)
            {
                _context.Groups.Add(model.Group);
            }
            else
            {
                var oldgroup = _context.Groups.FirstOrDefault(uu => uu.Id == model.Group.Id);
                oldgroup.Name = model.Group.Name;
                oldgroup.Event_ID = model.Group.Event_ID;
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Groups");
        }
        public ActionResult Edit(int id)
        {
            var model = _context.Groups.FirstOrDefault(uu => uu.Id == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            var viewmodel = new GroupsViewModel()
            {
                Events = _context.Events.ToList(),
                Group=model
            };
            return View("New", viewmodel);
        }
    }
}