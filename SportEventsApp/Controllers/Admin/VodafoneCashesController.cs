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
    public class VodafoneCashesController : Controller
    {
        ApplicationDbContext _context;
        public VodafoneCashesController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: EtisalatCashes
        public ActionResult Index()
        {
            var etcashes = _context.VodafoneCashs.Include(et => et.Event).ToList();
            return View(etcashes);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var vodafone = _context.VodafoneCashs.Include(uu => uu.Event).FirstOrDefault(use => use.Id == id);
            if (vodafone == null)
            {
                return HttpNotFound();
            }
            return View(vodafone);
        }
        public ActionResult New()
        {
            var events = _context.Events.ToList();

            var viewModel = new VodafoneViewModel()
            {
                Events = events,

                Vodafone = new VodafoneCash()
            };
            return View(viewModel);
        }
        public ActionResult Save(VodafoneViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new VodafoneViewModel()
                {
                    Events = _context.Events.ToList(),
                    Vodafone = model.Vodafone

                };
                return View("New", viewModel);
            }
            if (model.Vodafone.Id == 0)
            {
                _context.VodafoneCashs.Add(model.Vodafone);
            }
            else
            {
                var oldetisalat = _context.VodafoneCashs.FirstOrDefault(uu => uu.Id == model.Vodafone.Id);
                oldetisalat.Number = model.Vodafone.Number;
                oldetisalat.Event_ID = model.Vodafone.Event_ID;
                oldetisalat.Count = model.Vodafone.Count;

            }
            _context.SaveChanges();
            return RedirectToAction("Index", "VodafoneCashes");
        }

        public ActionResult Edit(int id)
        {
            var model = _context.VodafoneCashs.FirstOrDefault(uu => uu.Id == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            var viewmodel = new VodafoneViewModel()
            {
                Events = _context.Events.ToList(),
                Vodafone = model
            };
            return View("New", viewmodel);
        }

    }
}