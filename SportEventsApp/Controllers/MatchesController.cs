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
using System.ComponentModel.DataAnnotations;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;

namespace SportEventsApp.Controllers
{
    public class JoinMatchViewModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public int? MatchId { get; set; }
    }
    public class MatchesController : ApiController
    {
       

        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;
        private ApplicationDbContext db;
        public MatchesController()
        {
            db = new ApplicationDbContext();
        }
        public MatchesController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }



        // GET: api/Matches
        public async Task<IHttpActionResult> GetMatches()
        {

            //var boo = await UserManager.IsInRoleAsync("", "");
            var TEST = db.Matches.Include(mm => mm.Creator).ToList();
            var matches = db.Matches
                .Include(mm => mm.City)
                .Include(mm => mm.Creator)
                .Include(mm => mm.EntryFee)
                .Include(mm => mm.Store)
                .Include(mm => mm.Users)
                .Select(mm => new ReturnMatchViewModel
                {
                    CityName = mm.City.Name,
                    StoreOwnerId = mm.Store == null ? null : mm.Store.OwnerId,
                    CreatorId = mm.CreatorId,
                    CreatorName = mm.Creator == null ? null : mm.Creator.Name,
                    //CreatorRole = mm.Creator == null ? UserRoles.Admin : null,
                    CreatorStoreName = mm.Creator == null ? null : mm.Creator.Stores.FirstOrDefault().StoreName,
                    StoreName = mm.Store == null ? null : mm.Store.StoreName,
                    Date = mm.Date,
                    EntryFeeValue = mm.EntryFee.Value.Value,
                    Id = mm.Id,
                    NoOfSlots = mm.NofSlots.Value,
                    Prize = mm.Prize,
                    StoreAddress = mm.Store.Address,
                    StoreId = mm.StoreId == null ? 0 : mm.StoreId.Value,
                    Time = mm.Time,
                    Type = mm.Type,
                    Users = mm.Users.Select(uu => new MatchUsersViewModel
                    {
                        Id = uu.Id,
                        Mobile = uu.Mobile,
                        Name = uu.Name,
                        StoreName = uu.Stores.FirstOrDefault() == null ? null : uu.Stores.FirstOrDefault().StoreName,
                        UserName = uu.UserName
                    }).ToList()

                })
                .ToList();
            foreach (var item in matches)
            {
                item.CreatorRole = string.Join(", ", await UserManager.GetRolesAsync(item.CreatorId));
            }
            
            return Ok(matches);
        }

        [HttpGet]
        [Route("api/GetMatchesAdmin")]
        public IHttpActionResult GetMatchesAdmin()
        {
            var matches = db.Matches
                .Include(mm => mm.City)
                .Include(mm => mm.Creator)
                .Select(mm => new MatchesIndex
                {
                    Id = mm.Id,
                    CityName = mm.City.Name,
                    CreatorName = mm.Creator.Name == null ? mm.Store.StoreName : mm.Creator.Name,
                    CreatorUserName = mm.Creator.Mobile,
                    Date = mm.Date,
                    Time = mm.Time,

                }).ToList();

            return Ok(matches);
        }

        // GET: api/Matches/5
        [ResponseType(typeof(Match))]
        public IHttpActionResult GetMatch(int id)
        {
            var match = db.Matches
                .Include(mm => mm.City)
                .Include(mm => mm.Creator)
                .Include(mm => mm.EntryFee)
                .Include(mm => mm.Store)
                .Include(mm => mm.Users)
                .Select(mm => new ReturnMatchViewModel
                {
                    CityName = mm.City.Name,
                    StoreOwnerId = mm.Store==null ? null : mm.Store.OwnerId,
                    CreatorId = mm.CreatorId,
                    CreatorName = mm.Creator == null ? null : mm.Creator.Name,
                    CreatorRole = mm.Creator == null ? UserRoles.Admin : null,
                    CreatorStoreName = mm.Creator == null ? null : mm.Creator.Stores.FirstOrDefault().StoreName,
                    StoreName = mm.Store == null ? null : mm.Store.StoreName,
                    Date = mm.Date,
                    EntryFeeValue = mm.EntryFee.Value.Value,
                    Id = mm.Id,
                    NoOfSlots = mm.NofSlots.Value,
                    Prize = mm.Prize,
                    StoreAddress = mm.Store.Address,
                    StoreId = mm.StoreId == null ? 0 : mm.StoreId.Value,
                    Time = mm.Time,
                    Type = mm.Type,
                    Users = mm.Users.Select(uu => new MatchUsersViewModel
                    {
                        Id = uu.Id,
                        Mobile = uu.Mobile,
                        Name = uu.Name,
                        StoreName = uu.Stores.FirstOrDefault() == null ? null : uu.Stores.FirstOrDefault().StoreName,
                        UserName = uu.UserName
                    }).ToList()

                }).SingleOrDefault(m => m.Id == id);
            if (match == null)
            {
                return NotFound();
            }

            return Ok(match);
        }

        // PUT: api/Matches/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMatch(int id, MatchViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != model.Id)
            {
                return BadRequest();
            }
            var dbmatch = db.Matches.SingleOrDefault(mm => mm.Id == id);
            if (dbmatch == null)
            {
                return NotFound();
            }
            dbmatch.CityId = model.CityId;
            dbmatch.Date = model.Date.Value;
            dbmatch.EntryFeesId = model.EntryFeesId;
            dbmatch.NofSlots = model.NofSlots;
            dbmatch.Prize = model.Prize;
            dbmatch.StoreId = model.StoreId;
            dbmatch.Type = model.Type;
            dbmatch.CreatorId = model.CreatorId;
            dbmatch.Time = model.Time;
            try
            {
                db.SaveChanges();
                db.Matches.Attach(dbmatch);
                db.Entry(dbmatch).Reference(c => c.City).Load();
                db.Entry(dbmatch).Reference(c => c.Store).Load();
                db.Entry(dbmatch).Reference(c => c.Creator).Load();
                db.Entry(dbmatch).Reference(c => c.EntryFee).Load();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(HttpStatusCode.NotModified);
            }
            var viewModel = new ReturnMatchViewModel()
            {
                Id = dbmatch.Id,
                CityName = dbmatch.City.Name,
                StoreOwnerId = dbmatch.Store.OwnerId,
                CreatorId = dbmatch.CreatorId,
                CreatorName = dbmatch.Creator.Name,
                CreatorStoreName = dbmatch.Creator.Stores.FirstOrDefault() == null ? null : dbmatch.Creator.Stores.FirstOrDefault().StoreName,
                EntryFeeValue = dbmatch.EntryFee.Value.Value,
                StoreName = dbmatch.Store.StoreName,
                Date = dbmatch.Date,
                NoOfSlots = dbmatch.NofSlots.Value,
                Prize = dbmatch.Prize,
                StoreAddress = dbmatch.Store.StoreName,
                StoreId = dbmatch.StoreId.Value,
                Time = dbmatch.Time,
                Type = dbmatch.Type,
                Users = dbmatch.Users.Select(uu => new MatchUsersViewModel
                {
                    Id = uu.Id,
                    Mobile = uu.Mobile,
                    Name = uu.Name,
                    StoreName = uu.Stores.FirstOrDefault() == null ? null : uu.Stores.FirstOrDefault().StoreName,
                    UserName = uu.UserName
                }).ToList()
            };

            return Ok(dbmatch);
        }

        // POST: api/Matches
        [ResponseType(typeof(Match))]
        public IHttpActionResult PostMatch(MatchViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var match = new Match()
            {
                CityId = model.CityId,
                Date = model.Date.Value,
                EntryFeesId = model.EntryFeesId,
                NofSlots = model.NofSlots,
                Prize = model.Prize,
                StoreId = model.StoreId,
                Type = model.Type,
                CreatorId = model.CreatorId,
                Time = model.Time
                

            };
            var creator = db.Users.SingleOrDefault(su => su.Id == model.CreatorId);
            if (creator == null)
            {
                return NotFound();
            }
            match.Users.Add(creator);

            db.Matches.Add(match);


            try
            {
                db.SaveChanges();
                db.Matches.Attach(match);
                db.Entry(match).Reference(c => c.City).Load();
                db.Entry(match).Reference(c => c.Store).Load();
                db.Entry(match).Reference(c => c.Creator).Load();
                db.Entry(match).Reference(c => c.EntryFee).Load();

            }
            catch (Exception ex)
            {
                return StatusCode(HttpStatusCode.ExpectationFailed); //417
            }
            


            var viewModel = new ReturnMatchViewModel()
            {
                Id = match.Id,
                CityName = match.City.Name,
                StoreOwnerId = match.Store.OwnerId,
                CreatorId = match.CreatorId,
                CreatorName = match.Creator.Name,
                CreatorStoreName = match.Creator.Stores.FirstOrDefault() == null ? null : match.Creator.Stores.FirstOrDefault().StoreName,
                EntryFeeValue = match.EntryFee.Value.Value,
                StoreName = match.Store.StoreName,
                Date = match.Date,
                NoOfSlots = match.NofSlots.Value,
                Prize = match.Prize,
                StoreAddress = match.Store.StoreName,
                StoreId = match.StoreId.Value,
                Time = match.Time,
                Type = match.Type,
                Users = match.Users.Select(uu => new MatchUsersViewModel
                {
                    Id = uu.Id,
                    Mobile = uu.Mobile,
                    Name = uu.Name,
                    StoreName = uu.Stores.FirstOrDefault() == null ? null : uu.Stores.FirstOrDefault().StoreName,
                    UserName = uu.UserName
                }).ToList()
            };

            return Created(new Uri(Request.RequestUri + "/" + viewModel.Id), viewModel); //201
        }

        [HttpPost]
        [Route("api/JoinMatch")]
        public IHttpActionResult JoinMatch(JoinMatchViewModel model)
        {
            if (model.MatchId == null || model.UserId == null)
            {
                return BadRequest(ModelState); //400
            }
            var match = db.Matches.Include(mm => mm.Users).SingleOrDefault(mm => mm.Id == model.MatchId);
            var user = db.Users.SingleOrDefault(mm => mm.Id == model.UserId);

            if (match==null || user==null)
            {
                return NotFound(); //404
            }
            if (match.NofSlots == match.Users.Count)
            {
                return StatusCode(HttpStatusCode.Forbidden); //403
            }
            if (match.Users.Contains(user))
            {
                return StatusCode(HttpStatusCode.Found); //302
            }
            match.Users.Add(user);
            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                return StatusCode(HttpStatusCode.ExpectationFailed); //417
            }
            return Ok(model); //200

        }

        // DELETE: api/Matches/5
        [ResponseType(typeof(Match))]
        public IHttpActionResult DeleteMatch(int id)
        {
            Match match = db.Matches.Find(id);
            if (match == null)
            {
                return NotFound();
            }
            match.Users.ForEach(ss => ss.Matches.Clear());
            match.Users.Clear();
            db.Matches.Remove(match);
            db.SaveChanges();

            return Ok(match);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MatchExists(int id)
        {
            return db.Matches.Count(e => e.Id == id) > 0;
        }
    }
}