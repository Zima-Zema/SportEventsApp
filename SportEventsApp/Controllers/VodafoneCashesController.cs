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
using System.Web.Http.Cors;

namespace SportEventsApp.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class VodafoneCashesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/VodafoneCashes
        public IHttpActionResult GetVodafoneCashs()
        {
            var models = db.VodafoneCashs.Include(vc => vc.Event).ToList();
            return Ok(models);
        }

        // GET: api/VodafoneCashes/5
        [ResponseType(typeof(VodafoneCash))]
        public IHttpActionResult GetVodafoneCash(int id)
        {
            VodafoneCash vodafoneCash = db.VodafoneCashs.Include(vc => vc.Event).SingleOrDefault();
            if (vodafoneCash == null)
            {
                return NotFound();
            }

            return Ok(vodafoneCash);
        }

        // PUT: api/VodafoneCashes/5
        [ResponseType(typeof(VodafoneCash))]
        public IHttpActionResult PutVodafoneCash(int id, VodafoneCash vodafoneCash)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != vodafoneCash.Id)
            {
                return BadRequest();
            }

            db.Entry(vodafoneCash).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VodafoneCashExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(HttpStatusCode.NotModified);
                }
            }

            return Ok(vodafoneCash);
        }

        // POST: api/VodafoneCashes
        [ResponseType(typeof(VodafoneCash))]
        public IHttpActionResult PostVodafoneCash(VodafoneCash vodafoneCash)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.VodafoneCashs.Add(vodafoneCash);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = vodafoneCash.Id }, vodafoneCash);
        }

        // DELETE: api/VodafoneCashes/5
        [ResponseType(typeof(VodafoneCash))]
        public IHttpActionResult DeleteVodafoneCash(int id)
        {
            VodafoneCash vodafoneCash = db.VodafoneCashs.Find(id);
            if (vodafoneCash == null)
            {
                return NotFound();
            }

            db.VodafoneCashs.Remove(vodafoneCash);
            db.SaveChanges();

            return Ok(vodafoneCash);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VodafoneCashExists(int id)
        {
            return db.VodafoneCashs.Count(e => e.Id == id) > 0;
        }
    }
}