using Microsoft.AspNet.Identity;
using SportEventsApp.Models;
using SportEventsApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportEventsApp.Controllers.Admin
{
    public class MatchesController : Controller
    {
        ApplicationDbContext _context;
        public MatchesController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Matches
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult New()
        {
            var cities = _context.Cities.Select(c => new CitiesDropDown { Id = c.Id, Name = c.Name }).ToList();
            var fees = _context.EntryFees.ToList();

            var viewModel = new NewMatchViewModel()
            {
                Id = 0,
                Cities = cities,
                Fees = fees,

            };
            return View(viewModel);
        }

        public ActionResult Edit(int id)
        {
            var model = _context.Matches.SingleOrDefault(cu => cu.Id == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            var cities = _context.Cities.Select(c => new CitiesDropDown { Id = c.Id, Name = c.Name }).ToList();
            var fees = _context.EntryFees.ToList();

            var viewModel = new NewMatchViewModel()
            {
                Id = model.Id,
                CityId=model.CityId,
                CreatorId=model.CreatorId,
                Date=model.Date,
                EntryFeesId=model.EntryFeesId,
                NofSlots=model.NofSlots,
                Prize=model.Prize,
                StoreId=model.StoreId,
                Time=model.Time,
                Type=model.Type,
                Cities = cities,
                Fees = fees,

            };
            return View("New", viewModel);
        }

        public ActionResult Save(NewMatchViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var cities = _context.Cities.Select(c => new CitiesDropDown { Id = c.Id, Name = c.Name }).ToList();
                var fees = _context.EntryFees.ToList();

                model.Cities = cities;
                model.Fees = fees;

                return View("New", model);
            }
            if (model.Id == 0)
            {
                var match = new Match()
                {
                    Date = model.Date.Value,
                    Time = model.Time.Value,
                    NofSlots = model.NofSlots,
                    Type = model.Type,
                    Prize = model.Prize,
                    CityId = model.CityId,
                    CreatorId = User.Identity.GetUserId(),
                    EntryFeesId = model.EntryFeesId,
                    StoreId = model.StoreId
                };
                _context.Matches.Add(match);
            }
            else
            {
                var dbMatch = _context.Matches.SingleOrDefault(mm => mm.Id == model.Id);
                dbMatch.Date = model.Date.Value;
                dbMatch.Time = model.Time.Value;
                dbMatch.NofSlots = model.NofSlots;
                dbMatch.Type = model.Type;
                dbMatch.Prize = model.Prize;
                dbMatch.CityId = model.CityId;
                dbMatch.CreatorId = User.Identity.GetUserId();
                dbMatch.EntryFeesId = model.EntryFeesId;
                dbMatch.StoreId = model.StoreId;

            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Matches");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}