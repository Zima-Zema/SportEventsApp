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
    public class EventsController : Controller
    {
        private ApplicationDbContext _context;
        public EventsController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Events
   
        public ActionResult Index()
        {
            var events = _context.Events.ToList();
            return View(events);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event ev = _context.Events.FirstOrDefault(e => e.Id == id);
            if (ev == null)
            {
                return HttpNotFound();
            }
            return View(ev);
        }

        public ActionResult New()
        {
            var ev = new EventsViewModel();

            return View(ev);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(EventsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("New", model);
            }
            if (model.Id == 0)
            {
                var @event = new Event();
                @event.Name = model.Name;
                @event.Prize_1 = model.Prize_1;
                @event.Prize_2 = model.Prize_2;
                @event.Prize_3 = model.Prize_3;
                @event.Host_1 = model.Host_1;
                @event.Host_2 = model.Host_2;
                @event.Host_3 = model.Host_3;
                @event.Date = model.Date.Date;
                @event.Time = model.Time;
                @event.Entry_Fees = model.Entry_Fees;
                @event.No_Of_Players = model.No_Of_Players;
                @event.Type = model.Type;
                @event.Match_Duration = model.Match_Duration;
                @event.Address = model.Address;
                @event.Location_URL = model.Location_URL;

                _context.Events.Add(@event);
                _context.SaveChanges();
                foreach (var item in model.VodafoneCashNumbers)
                {
                    var Vodafone = new VodafoneCash();
                    Vodafone.Number = item;
                    Vodafone.Count = 0;
                    Vodafone.Event_ID = @event.Id;
                    _context.VodafoneCashs.Add(Vodafone);
                }
                foreach (var item in model.EtisalatCashNumbers)
                {
                    var Etisalat = new EtisalatCash();
                    Etisalat.Number = item;
                    Etisalat.Count = 0;
                    Etisalat.Event_ID = @event.Id;
                    _context.EtisalatCashs.Add(Etisalat);

                }

            }
            else
            {
                var dbevent = _context.Events.SingleOrDefault(e => e.Id == model.Id);
                dbevent.Name = model.Name;
                dbevent.Prize_1 = model.Prize_1;
                dbevent.Prize_2 = model.Prize_2;
                dbevent.Prize_3 = model.Prize_3;
                dbevent.Host_1 = model.Host_1;
                dbevent.Host_2 = model.Host_2;
                dbevent.Host_3 = model.Host_3;
                dbevent.Date = model.Date.Date;
                dbevent.Time = model.Time;
                dbevent.Entry_Fees = model.Entry_Fees;
                dbevent.No_Of_Players = model.No_Of_Players;
                dbevent.Type = model.Type;
                dbevent.Match_Duration = model.Match_Duration;
                dbevent.Address = model.Address;
                dbevent.Location_URL = model.Location_URL;

                for (int i = 0; i < model.VodafoneCashNumbers.Count; i++)
                {
                    var v = _context.VodafoneCashs.Where(vo => vo.Event_ID == model.Id).OrderBy(vf => vf.Id).Skip(i).Take(1).FirstOrDefault();
                    if (v != null)
                    {
                        v.Number = model.VodafoneCashNumbers[i];
                    }
                    else
                    {
                        var Vodafone = new VodafoneCash();
                        Vodafone.Number = model.VodafoneCashNumbers[i];
                        Vodafone.Count = 0;
                        Vodafone.Event_ID = model.Id;
                        _context.VodafoneCashs.Add(Vodafone);
                    }
                }

                for (int i = 0; i < model.EtisalatCashNumbers.Count; i++)
                {
                    var e = _context.EtisalatCashs.Where(et => et.Event_ID == model.Id).OrderBy(vf => vf.Id).Skip(i).Take(1).FirstOrDefault();
                    if (e != null)
                    {
                        e.Number = model.EtisalatCashNumbers[i];
                    }
                    else
                    {
                        var Etisalat = new EtisalatCash();
                        Etisalat.Number = model.EtisalatCashNumbers[i];
                        Etisalat.Count = 0;
                        Etisalat.Event_ID = model.Id;
                        _context.EtisalatCashs.Add(Etisalat);
                    }

                }
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Events");

        }

        public ActionResult Edit(int id)
        {
            var model = _context.Events.SingleOrDefault(cu => cu.Id == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            var vCashList = _context.VodafoneCashs.Where(v => v.Event_ID == model.Id).Select(vc => vc.Number).ToList();
            var eCashList = _context.EtisalatCashs.Where(et => et.Event_ID == model.Id).Select(ee => ee.Number).ToList();
            var viewModel = new EventsViewModel
            {
                Id = model.Id,
                Name = model.Name,
                Prize_1 = model.Prize_1,
                Prize_2 = model.Prize_2,
                Prize_3 = model.Prize_3,
                Host_1 = model.Host_1,
                Host_2 = model.Host_2,
                Host_3 = model.Host_3,
                Date = model.Date.Date,
                Time = model.Time,
                Entry_Fees = model.Entry_Fees,
                No_Of_Players = model.No_Of_Players,
                Type = model.Type,
                Match_Duration = model.Match_Duration,
                Address = model.Address,
                Location_URL = model.Location_URL,
                EtisalatCashNumbers = eCashList,
                VodafoneCashNumbers = vCashList

            };
            return View("New", viewModel);
        }
    }
}