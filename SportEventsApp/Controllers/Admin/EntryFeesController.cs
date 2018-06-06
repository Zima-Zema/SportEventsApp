using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SportEventsApp.Models;

namespace SportEventsApp.Controllers.Admin
{
    public class EntryFeesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: EntryFees
        public ActionResult Index()
        {
            return View(db.EntryFees.ToList());
        }

        // GET: EntryFees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EntryFees entryFees = db.EntryFees.Find(id);
            if (entryFees == null)
            {
                return HttpNotFound();
            }
            return View(entryFees);
        }

        // GET: EntryFees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EntryFees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Value")] EntryFees entryFees)
        {
            if (ModelState.IsValid)
            {
                entryFees.Name = entryFees.Value.ToString();
                db.EntryFees.Add(entryFees);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(entryFees);
        }

        // GET: EntryFees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EntryFees entryFees = db.EntryFees.Find(id);
            if (entryFees == null)
            {
                return HttpNotFound();
            }
            return View(entryFees);
        }

        // POST: EntryFees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Value")] EntryFees entryFees)
        {
            if (ModelState.IsValid)
            {
                var fees = db.EntryFees.SingleOrDefault(ff=>ff.Id == entryFees.Id);
                fees.Name = entryFees.Value.ToString();
                fees.Value = entryFees.Value;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(entryFees);
        }

        // GET: EntryFees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EntryFees entryFees = db.EntryFees.Find(id);
            if (entryFees == null)
            {
                return HttpNotFound();
            }
            return View(entryFees);
        }

        // POST: EntryFees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EntryFees entryFees = db.EntryFees.Find(id);
            db.EntryFees.Remove(entryFees);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
