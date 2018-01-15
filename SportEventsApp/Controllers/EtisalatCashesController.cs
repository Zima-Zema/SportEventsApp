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
    public class EtisalatCashesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/EtisalatCashes
        public IHttpActionResult GetEtisalatCashs()
        {
            var models = db.EtisalatCashs.Include(ec => ec.Event).ToList();
            return Ok(models);
        }

        // GET: api/EtisalatCashes/5
        [ResponseType(typeof(EtisalatCash))]
        public IHttpActionResult GetEtisalatCash(int id)
        {
            EtisalatCash etisalatCash = db.EtisalatCashs.Include(ec => ec.Event).SingleOrDefault();
            if (etisalatCash == null)
            {
                return NotFound();
            }

            return Ok(etisalatCash);
        }

        // PUT: api/EtisalatCashes/5
        [ResponseType(typeof(EtisalatCash))]
        public IHttpActionResult PutEtisalatCash(int id, EtisalatCash etisalatCash)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != etisalatCash.Id)
            {
                return BadRequest();
            }

            db.Entry(etisalatCash).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EtisalatCashExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(HttpStatusCode.NotModified);
                }
            }

            return Ok(etisalatCash);
        }

        // POST: api/EtisalatCashes
        [ResponseType(typeof(EtisalatCash))]
        public IHttpActionResult PostEtisalatCash(EtisalatCash etisalatCash)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EtisalatCashs.Add(etisalatCash);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = etisalatCash.Id }, etisalatCash);
        }

        // DELETE: api/EtisalatCashes/5
        [ResponseType(typeof(EtisalatCash))]
        public IHttpActionResult DeleteEtisalatCash(int id)
        {
            EtisalatCash etisalatCash = db.EtisalatCashs.Find(id);
            if (etisalatCash == null)
            {
                return NotFound();
            }

            db.EtisalatCashs.Remove(etisalatCash);
            db.SaveChanges();

            return Ok(etisalatCash);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EtisalatCashExists(int id)
        {
            return db.EtisalatCashs.Count(e => e.Id == id) > 0;
        }
    }
}