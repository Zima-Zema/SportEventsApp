using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using SportEventsApp.Models;
using SportEventsApp.ViewModel;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace SportEventsApp.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EventsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Events
        public IHttpActionResult GetEvents()
        {
            var events = db.Events
                .Where(ee => ee.Published == true)
                .Include(ev => ev.VodafoneCashNumbers)
                .Include(ee => ee.EtisalatCashNumbers)
                .Include(ee=>ee.Stores)
                .Include(e => e.EventUsers)
                .Select(e=> new ReturnEventViewModel
                {
                    Id=e.Id,
                    Name=e.Name,
                    Prize1=e.Prize_1,
                    Prize2=e.Prize_2,
                    Prize3=e.Prize_3,
                    Published=e.Published,
                    End=e.End,
                    Start=e.Start,
                    From=e.From,
                    EntryFees=e.Entry_Fees,
                    Full=e.Full,
                    MatchDuration=e.MatchDuration,
                    NoOfPlayers=e.No_Of_Players,
                    To=e.To,
                    Type =e.Type,
                    Stores=e.Stores.Select(s=> new ReturnEventStores
                    {
                        storeOwnerId=s.OwnerId,
                        cityName = s.City.Name,
                        storeName=s.StoreName
                    }).ToList()
                })
                .ToList();
            return Ok(events);
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/GetEventsAdmin")]
        public IHttpActionResult GetEventsAdmin()
        {
            var events =  db.Events
                
                .Include(ev => ev.VodafoneCashNumbers)
                .Include(ee => ee.EtisalatCashNumbers)
                .Include(ee => ee.Stores)
                .Include(e => e.EventUsers)
                .Select(e => new ReturnEventViewModel
                {
                    Id = e.Id,
                    Name = e.Name,
                    Prize1 = e.Prize_1,
                    Prize2 = e.Prize_2,
                    Prize3 = e.Prize_3,
                    Published = e.Published,
                    End = e.End,
                    Start = e.Start,
                    From = e.From,
                    EntryFees = e.Entry_Fees,
                    Full = e.Full,
                    MatchDuration = e.MatchDuration,
                    NoOfPlayers = e.No_Of_Players,
                    To = e.To,
                    Type = e.Type,
                }).ToList();
            return Ok(events);
        }

        // GET: api/Events/5
        [ResponseType(typeof(ReturnEventViewModel))]
        public IHttpActionResult GetEvent(int id)
        {
            var @event = db.Events
                .Include(ev => ev.VodafoneCashNumbers)
                .Include(ee => ee.EtisalatCashNumbers)
                .Include(e => e.EventUsers)
                .Include(e => e.Stores)
                .Select(e => new ReturnEventViewModel
                {
                    Id = e.Id,
                    Name = e.Name,
                    Prize1 = e.Prize_1,
                    Prize2 = e.Prize_2,
                    Prize3 = e.Prize_3,
                    Published = e.Published,
                    End = e.End,
                    Start = e.Start,
                    From = e.From,
                    EntryFees = e.Entry_Fees,
                    Full = e.Full,
                    MatchDuration = e.MatchDuration,
                    NoOfPlayers = e.No_Of_Players,
                    To = e.To,
                    Type = e.Type,
                    Stores = e.Stores.Select(s => new ReturnEventStores
                    {
                        storeOwnerId = s.OwnerId,
                        cityName = s.City.Name,
                        storeName = s.StoreName
                    }).ToList()
                }).SingleOrDefault(eve => eve.Id == id);

            if (@event == null)
            {
                return NotFound();
            }

            return Ok(@event);
        }

        // PUT: api/Events/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEvent(int id, EventsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != model.Id)
            {
                return BadRequest();
            }

            var dbevent = db.Events.SingleOrDefault(e => e.Id == id);
            if (dbevent == null)
            {
                return NotFound();
            }

            dbevent.Name = model.Name;
            dbevent.Prize_1 = model.Prize_1.Value;
            dbevent.Prize_2 = model.Prize_2.Value;
            dbevent.Prize_3 = model.Prize_3.Value;
            dbevent.Entry_Fees = model.EntryFees.Value;
            dbevent.No_Of_Players = model.NoOfPlayers.Value;
            dbevent.Type = model.Type;

            dbevent.Location_URL = model.Location_URL;

            dbevent.Start = model.Start.Value.Date;
            dbevent.End = model.End.Value.Date;
            dbevent.From = model.From.Value;
            dbevent.To = model.To.Value;
            dbevent.Published = model.Published;


            for (int i = 0; i < model.VodafoneCashNumbers.Count; i++)
            {
                var v = db.VodafoneCashs.Where(vo => vo.Event_ID == model.Id).OrderBy(vf => vf.Id).Skip(i).Take(1).FirstOrDefault();
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
                    db.VodafoneCashs.Add(Vodafone);
                }
            }
            
            for (int i = 0; i < model.EtisalatCashNumbers.Count; i++)
            {
                var e = db.EtisalatCashs.Where(et => et.Event_ID == model.Id).OrderBy(vf => vf.Id).Skip(i).Take(1).FirstOrDefault();
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
                    db.EtisalatCashs.Add(Etisalat);
                }

            }
           
            try
            {

               int c = db.SaveChanges();

            }
            catch (Exception)
            {
                if (!EventExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(HttpStatusCode.NotModified);
                }
            }

            return Ok(model);
        }

        // POST: api/Events

        public IHttpActionResult PostEvent([FromBody]EventsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var @event = new Event();
            @event.Name = model.Name;
            @event.Prize_1 = model.Prize_1.Value;
            @event.Prize_2 = model.Prize_2.Value;
            @event.Prize_3 = model.Prize_3.Value;
            @event.Entry_Fees = model.EntryFees.Value;
            @event.No_Of_Players = model.NoOfPlayers.Value;
            @event.Type = model.Type;
            @event.Location_URL = model.Location_URL;
            @event.Start = model.Start.Value.Date;
            @event.End = model.End.Value.Date;
            @event.From = model.From.Value;
            @event.To = model.To.Value;
            @event.Published = false;

            db.Events.Add(@event);
            db.SaveChanges();
            foreach (var item in model.VodafoneCashNumbers)
            {
                var Vodafone = new VodafoneCash();
                Vodafone.Number = item;
                Vodafone.Count = 0;
                Vodafone.Event_ID = @event.Id;
                db.VodafoneCashs.Add(Vodafone);
            }
            foreach (var item in model.EtisalatCashNumbers)
            {
                var Etisalat = new EtisalatCash();
                Etisalat.Number = item;
                Etisalat.Count = 0;
                Etisalat.Event_ID = @event.Id;
                db.EtisalatCashs.Add(Etisalat);

            }
            db.SaveChanges();

            return Created(new Uri(Request.RequestUri + "/" + @event.Id), @event);
        }

        // DELETE: api/Events/5
        [ResponseType(typeof(Event))]
        public IHttpActionResult DeleteEvent(int id)
        {
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return NotFound();
            }
            @event.EtisalatCashNumbers.ForEach(et => et.Event_ID = null);
            @event.VodafoneCashNumbers.ForEach(v => v.Event_ID = null);
            @event.EventUsers.Clear();
            @event.Groups.ForEach(g => g.Event_ID = null);
            db.Events.Remove(@event);
            db.SaveChanges();

            return Ok(@event);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EventExists(int id)
        {
            return db.Events.Count(e => e.Id == id) > 0;
        }
    }
}