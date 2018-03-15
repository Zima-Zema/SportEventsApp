using SportEventsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using System.Net;
using SportEventsApp.ViewModel;

namespace SportEventsApp.Controllers.Admin
{
    [Authorize]
    public class EtisalatCashesController : Controller
    {
        ApplicationDbContext _context;
        public EtisalatCashesController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: EtisalatCashes
        public ActionResult Index()
        {
            var etcashes = _context.EtisalatCashs.Include(et => et.Event).ToList();
            return View(etcashes);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var etisalat = _context.EtisalatCashs.Include(uu => uu.Event).FirstOrDefault(use => use.Id == id);
            if (etisalat == null)
            {
                return HttpNotFound();
            }
            return View(etisalat);
        }
        public ActionResult New()
        {
            var events = _context.Events.ToList();

            var viewModel = new EtisalateViewModel()
            {
                Events = events,

                Etisalat = new EtisalatCash()
            };
            return View(viewModel);
        }
        public ActionResult Save(EtisalateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new EtisalateViewModel()
                {
                    Events = _context.Events.ToList(),
                    Etisalat = model.Etisalat

                };
                return View("New", viewModel);
            }
            if (model.Etisalat.Id == 0)
            {
                _context.EtisalatCashs.Add(model.Etisalat);
            }
            else
            {
                var oldetisalat = _context.EtisalatCashs.FirstOrDefault(uu => uu.Id == model.Etisalat.Id);
                oldetisalat.Number = model.Etisalat.Number;
                oldetisalat.Event_ID = model.Etisalat.Event_ID;
                oldetisalat.Count = model.Etisalat.Count;
               
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "EtisalatCashes");
        }

        public ActionResult Edit(int id)
        {
            var model = _context.EtisalatCashs.FirstOrDefault(uu => uu.Id == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            var viewmodel = new EtisalateViewModel()
            {
                Events = _context.Events.ToList(),
                Etisalat = model
            };
            return View("New", viewmodel);
        }

    }
}