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

namespace SportEventsApp.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EventsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Events
        public IHttpActionResult GetEvents()
        {
            var events = db.Events.Include(ev => ev.VodafoneCashNumbers).Include(ee => ee.EtisalatCashNumbers).ToList();
            return Ok(events);
        }

        // GET: api/Events/5
        [ResponseType(typeof(Event))]
        public IHttpActionResult GetEvent(int id)
        {
            Event @event = db.Events.Include(ev => ev.VodafoneCashNumbers).Include(ee => ee.EtisalatCashNumbers).SingleOrDefault(eve => eve.Id == id);
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
            dbevent.Prize_1 = model.Prize_1;
            dbevent.Prize_2 = model.Prize_2;
            dbevent.Prize_3 = model.Prize_3;
            dbevent.Host_1 = model.Host_1;
            dbevent.Host_2 = model.Host_2;
            dbevent.Host_3 = model.Host_3;
            //dbevent.Date = model.Date.Date;
            //dbevent.Time = model.Time;
            dbevent.Entry_Fees = model.Entry_Fees;
            dbevent.No_Of_Players = model.No_Of_Players;
            dbevent.Type = model.Type;
            dbevent.Match_Duration = model.Match_Duration;
            dbevent.Address = model.Address;
            dbevent.Location_URL = model.Location_URL;



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
            @event.Prize_1 = model.Prize_1;
            @event.Prize_2 = model.Prize_2;
            @event.Prize_3 = model.Prize_3;
            @event.Host_1 = model.Host_1;
            @event.Host_2 = model.Host_2;
            @event.Host_3 = model.Host_3;
            //@event.Date = model.Date.Date;
            //@event.Time = model.Time;
            @event.Entry_Fees = model.Entry_Fees;
            @event.No_Of_Players = model.No_Of_Players;
            @event.Type = model.Type;
            @event.Match_Duration = model.Match_Duration;
            @event.Address = model.Address;
            @event.Location_URL = model.Location_URL;

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
            //@event.Users.ForEach(u => u.Event_ID = null);
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