using SportEventsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using SportEventsApp.ViewModel;
using System.Net;

namespace SportEventsApp.Controllers.Admin
{
    [Authorize]
    public class UsersController : Controller
    {
        ApplicationDbContext _context;
        public UsersController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Users
        public ActionResult Index()
        {

            
            return View();
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //var user = _context.EUsers.Include(uu => uu.Event).Include(us => us.Group).FirstOrDefault(use => use.Id == id);
            //if (user == null)
            //{
            //    return HttpNotFound();
            //}
            return View();
        }
        public ActionResult New()
        {
            return View();
        }

        public ActionResult Save(UsersViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new UsersViewModel()
                {
                    Events = _context.Events.ToList(),
                    Groups = _context.Groups.ToList(),
                    User = model.User
                };
                return View("New", viewModel);
            }
            //if (model.User.Id == 0)
            //{
            //    _context.EUsers.Add(model.User);
            //}
            //else
            //{
            //    var olduser = _context.EUsers.FirstOrDefault(uu => uu.Id == model.User.Id);
            //    olduser.UserName = model.User.UserName;
            //    olduser.Mobile = model.User.Mobile;
            //    olduser.Status = model.User.Status;
            //    olduser.Event_ID = model.User.Event_ID;
            //    olduser.Group_ID = model.User.Group_ID;

            //}
            _context.SaveChanges();
            return RedirectToAction("Index", "Users");
        }

        public ActionResult Edit(int id)
        {
            //var model = _context.EUsers.FirstOrDefault(uu => uu.Id == id);
            //if (model == null)
            //{
            //    return HttpNotFound();
            //}
            //var viewmodel = new UsersViewModel()
            //{
            //    Events = _context.Events.ToList(),
            //    Groups = _context.Groups.ToList(),
            //    User = model
            //};
            return View("New");
        }
    }
}