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
            //var events = _context.Events.ToList();
            return View();
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
            var usertour = (from eu in _context.EventUsers
                            where eu.EventId == ev.Id
                                select new UserTournamentViewModel
                                {
                                    CashNumber = eu.CashNumber,
                                    GroupId = eu.GroupId,
                                    EventId = eu.EventId,
                                    UserId = eu.UserId,
                                    Status = eu.Status,
                                    Event = eu.Event,
                                    Group = eu.Group,
                                    User = eu.User,
                                    Groups = _context.Groups.Where(gg => gg.Event_ID == eu.EventId).ToList()
                                }
                           ).ToList();

            var viewModel = new EventsDetailsViewModel()
            {
                Event = ev,
                Tours = usertour
            };
            return View(viewModel);
        }

        public ActionResult New()
        {
            var stores = _context.Stores.Select(s => new StoreDropdown { Id=s.Id, StoreName=s.StoreName }).ToList();
            var ev = new EventsViewModel()
            {
                Stores=stores
            };

            return View(ev);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(EventsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var stores = _context.Stores.Select(s => new StoreDropdown { Id = s.Id, StoreName = s.StoreName }).ToList();
                model.Stores = stores;
                return View("New", model);
            }
            if (model.Id == 0)
            {
                var @event = new Event();
                @event.Name = model.Name;

                @event.Prize_1 = model.Prize_1.Value;
                @event.Prize_2 = model.Prize_2.Value;
                @event.Prize_3 = model.Prize_3.Value;

                @event.Start = model.Start.Value.Date;
                @event.End = model.End.Value.Date;

                @event.From = model.From.Value;
                @event.To = model.To.Value;
                
                @event.Entry_Fees = model.EntryFees.Value;
                @event.No_Of_Players = model.NoOfPlayers.Value;
                @event.Type = model.Type;

                @event.Published = model.Published.Value; 

                @event.Location_URL = model.Location_URL;
                @event.MatchDuration = model.Match_Duration.Value;


                _context.Events.Add(@event);
                _context.SaveChanges();

                var fStore = _context.Stores.SingleOrDefault(s => s.Id == model.Store1.Value);
                if (fStore != null)
                {
                    @event.Stores.Add(fStore);
                }
                var sStore = _context.Stores.SingleOrDefault(s => s.Id == model.Store2.Value);
                if (sStore != null)
                {
                    @event.Stores.Add(sStore);
                }
                var tStore = _context.Stores.SingleOrDefault(s => s.Id == model.Store3.Value);
                if (tStore != null)
                {
                    @event.Stores.Add(tStore);
                }


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
                dbevent.Prize_1 = model.Prize_1.Value;
                dbevent.Prize_2 = model.Prize_2.Value;
                dbevent.Prize_3 = model.Prize_3.Value;


                dbevent.Start = model.Start.Value.Date;
                dbevent.End = model.End.Value.Date;

                dbevent.From = model.From.Value;
                dbevent.To = model.To.Value;

                dbevent.Published = model.Published.Value;

                dbevent.Entry_Fees = model.EntryFees.Value;
                dbevent.No_Of_Players = model.NoOfPlayers.Value;
                dbevent.Type = model.Type;
                dbevent.MatchDuration = model.Match_Duration.Value;
                dbevent.Location_URL = model.Location_URL;

                dbevent.Stores.Clear();

                var fStore = _context.Stores.SingleOrDefault(s => s.Id == model.Store1.Value);
                if (fStore != null)
                {
                    dbevent.Stores.Add(fStore);
                }
                var sStore = _context.Stores.SingleOrDefault(s => s.Id == model.Store2.Value);
                if (sStore != null)
                {
                    dbevent.Stores.Add(sStore);
                }
                var tStore = _context.Stores.SingleOrDefault(s => s.Id == model.Store3.Value);
                if (tStore != null)
                {
                    dbevent.Stores.Add(tStore);
                }

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
            var stores = _context.Stores.Select(s => new StoreDropdown { Id = s.Id, StoreName = s.StoreName }).ToList();
            var viewModel = new EventsViewModel
            {
                Id = model.Id,
                Name = model.Name,
                Prize_1 = model.Prize_1,
                Prize_2 = model.Prize_2,
                Prize_3 = model.Prize_3,
                From = model.From,
                To = model.To,
                End = model.End,
                Start = model.Start,
                Match_Duration = model.MatchDuration,
                Published = model.Published,
                Store1 = model.Stores.FirstOrDefault() == null ? (int?)null : model.Stores.FirstOrDefault().Id,
                Store2 = model.Stores.Skip(1).FirstOrDefault() == null ? (int?)null : model.Stores.Skip(1).FirstOrDefault().Id,
                Store3 = model.Stores.Skip(2).FirstOrDefault() == null ? (int?)null : model.Stores.Skip(2).FirstOrDefault().Id,
                Stores = stores,

                EntryFees = model.Entry_Fees,
                NoOfPlayers = model.No_Of_Players,
                Type = model.Type,

                Location_URL = model.Location_URL,
                EtisalatCashNumbers = eCashList,
                VodafoneCashNumbers = vCashList

            };
            return View("New", viewModel);
        }
    }


}