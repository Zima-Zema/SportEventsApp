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

namespace SportEventsApp.Controllers
{
    public class EntryFeesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/EntryFees
        public IHttpActionResult GetEntryFees()
        {
            var fees = db.EntryFees.ToList();
            return Ok(fees);
        }

        // GET: api/EntryFees/5
        [ResponseType(typeof(EntryFees))]
        public IHttpActionResult GetEntryFees(int id)
        {
            EntryFees entryFees = db.EntryFees.Find(id);
            if (entryFees == null)
            {
                return NotFound();
            }

            return Ok(entryFees);
        }

        // PUT: api/EntryFees/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEntryFees(int id, EntryFees entryFees)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != entryFees.Id)
            {
                return BadRequest();
            }

            db.Entry(entryFees).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntryFeesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/EntryFees
        [ResponseType(typeof(EntryFees))]
        public IHttpActionResult PostEntryFees(EntryFees entryFees)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.EntryFees.Add(entryFees);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = entryFees.Id }, entryFees);
        }

        // DELETE: api/EntryFees/5
        [ResponseType(typeof(EntryFees))]
        public IHttpActionResult DeleteEntryFees(int id)
        {
            EntryFees entryFees = db.EntryFees.Find(id);
            if (entryFees == null)
            {
                return NotFound();
            }

            db.EntryFees.Remove(entryFees);
            db.SaveChanges();

            return Ok(entryFees);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EntryFeesExists(int id)
        {
            return db.EntryFees.Count(e => e.Id == id) > 0;
        }
    }
}