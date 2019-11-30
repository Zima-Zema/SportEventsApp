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
using System.ComponentModel.DataAnnotations;
using SportEventsApp.ViewModel;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;

namespace SportEventsApp.Controllers
{
    public class StoreViewModel
    {

        public int Id { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string StoreName { get; set; }
        [Required]
        public string Address { get; set; }
        [Display(Name = "Number Of Devices")]
        public int? NumberOfDevices { get; set; }
        public double? HoureFees { get; set; }
        [Required]
        public string OpenTime { get; set; }
        [Required]
        public string CloseTime { get; set; }
        [Required]
        public DayOfWeek From { get; set; }
        [Required]
        public DayOfWeek To { get; set; }
        public bool? Approved { get; set; } = true;
        [Required]
        public string OwnerId { get; set; }
        [Required]
        public string CityId { get; set; }
        public List<string> Photos { get; set; }
        public string UserName { get; set; }




    }
    public class StoresController : ApiController
    {
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;
        private ApplicationDbContext db;
        public StoresController()
        {
            db = new ApplicationDbContext();
        }
        public StoresController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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






        // GET: api/Stores
        public IHttpActionResult GetStores()
        {
            var stores = db.Stores.Where(s => s.Approved == true)
                .Include(s => s.Owner)
                .Include(s => s.Matches)
                .Include(e => e.Events)
                .Include(ss => ss.City)
                .Select(ss => new StoreReturn
                {
                    Address = ss.Address,
                    Approved = ss.Approved,
                    City = ss.City,
                    CloseTime = ss.CloseTime,
                    Events = ss.Events.Select(e => new ReturnEventViewModel
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

                    }).ToList(),
                    From = ss.From,
                    HoureFees = ss.HoureFees,
                    Id = ss.Id,
                    Matches = ss.Matches.Select(mm => new ReturnMatchViewModel
                    {
                        CityName = mm.City.Name,
                        StoreOwnerId = mm.Store.OwnerId,
                        CreatorId = mm.CreatorId,
                        CreatorName = mm.Creator.Name,
                        CreatorStoreName = mm.Creator.Stores.FirstOrDefault() == null ? null : mm.Creator.Stores.FirstOrDefault().StoreName,
                        StoreName = mm.Store.StoreName,
                        Date = mm.Date,
                        EntryFeeValue = mm.EntryFee.Value.Value,
                        Id = mm.Id,
                        NoOfSlots = mm.NofSlots.Value,
                        Prize = mm.Prize,
                        StoreAddress = mm.Store.Address,
                        StoreId = mm.StoreId.Value,
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
                    }).ToList(),
                    NumberOfDevices = ss.NumberOfDevices,
                    OpenTime = ss.OpenTime,
                    Photos=ss.Photos,
                    Role=UserRoles.StoreOwner,
                    StoreName=ss.StoreName,
                    To=ss.To,
                    UserName=ss.Owner.UserName,
                    ValidUser=ss.Owner.ValidUser
                }).ToList();
            return Ok(stores);
        }

        // GET: api/Stores/5
        [ResponseType(typeof(Store))]
        public IHttpActionResult GetStore(int id)
        {
            Store store = db.Stores
                .Include(s => s.Owner)
                .Include(s => s.Matches)
                .Include(e => e.Events)
                .Include(ss => ss.City)
                .SingleOrDefault(st => st.Id == id);


            var resp = new StoreReturn
            {
                Address = store.Address,
                Approved = store.Approved,
                City = store.City,
                CloseTime = store.CloseTime,
                Events = store.Events.Select(e => new ReturnEventViewModel
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

                }).ToList(),
                From = store.From,
                HoureFees = store.HoureFees,
                Id = store.Id,
                Matches = store.Matches.Select(mm => new ReturnMatchViewModel
                {
                    CityName = mm.City.Name,
                    StoreOwnerId = mm.Store.OwnerId,
                    CreatorId = mm.CreatorId,
                    CreatorName = mm.Creator.Name,
                    CreatorStoreName = mm.Creator.Stores.FirstOrDefault() == null ? null : mm.Creator.Stores.FirstOrDefault().StoreName,
                    StoreName = mm.Store.StoreName,
                    Date = mm.Date,
                    EntryFeeValue = mm.EntryFee.Value.Value,
                    Id = mm.Id,
                    NoOfSlots = mm.NofSlots.Value,
                    Prize = mm.Prize,
                    StoreAddress = mm.Store.Address,
                    StoreId = mm.StoreId.Value,
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
                }).ToList(),
                NumberOfDevices = store.NumberOfDevices,
                OpenTime = store.OpenTime,
                Photos = store.Photos,
                Role = UserRoles.StoreOwner,
                StoreName = store.StoreName,
                To = store.To,
                UserName = store.Owner.UserName,
                ValidUser = store.Owner.ValidUser
            };

            if (store == null)
            {
                return NotFound();
            }

            return Ok(resp);
        }

        [HttpGet]
        [Route("api/GetStoreByCityId/{id}")]
        public IHttpActionResult GetStoreByCityId(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var stores = db.Stores.Where(s => s.CityId == id && s.Approved == true).Select(ss=>new { ss.Id,ss.StoreName }).ToList();
            return Ok(stores);
        }

        // PUT: api/Stores/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutStore(int id, StoreViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != model.Id)
            {
                return BadRequest();
            }


            var appUser = await UserManager.FindByIdAsync(model.OwnerId);
            appUser.UserName = appUser.Mobile = model.UserName;

            appUser.ValidUser = true;

            var res = await UserManager.UpdateAsync(appUser);

            if (res.Errors.Contains($"Name {model.UserName} is already taken."))
            {
                return Conflict();
            }
            var store = db.Stores.SingleOrDefault(ss => ss.Id == model.Id);
            if (store == null)
            {
                return NotFound();
            }
            store.Address = model.Address;
            store.Approved = model.Approved;
            store.CityId = model.CityId;
            store.CloseTime = model.CloseTime;
            store.From = model.From;
            store.HoureFees = model.HoureFees;
            store.NumberOfDevices = model.NumberOfDevices;
            store.OpenTime = model.OpenTime;
            store.OwnerId = model.OwnerId;
            store.StoreName = model.StoreName;
            store.To = model.To;
            store.WorkingHours = DateTime.Parse(model.CloseTime).Subtract(DateTime.Parse(model.OpenTime));

            for (int i = 0; i < model.Photos.Count; i++)
            {
                var v = db.StorePhotos.Where(vo => vo.StoreId == model.Id).OrderBy(vf => vf.Id).Skip(i).Take(1).FirstOrDefault();
                if (v != null)
                {
                    v.Url = model.Photos[i];
                }
                else
                {
                    var storephoto = new StorePhotos();
                    storephoto.Url = model.Photos[i];
                    storephoto.StoreId = model.Id;
                    db.StorePhotos.Add(storephoto);
                }
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
               return StatusCode(HttpStatusCode.NotModified);
            }
            var resp = new StoreReturn
            {
                Address = store.Address,
                Approved = store.Approved,
                City = store.City,
                CloseTime = store.CloseTime,
                Events = store.Events.Select(e => new ReturnEventViewModel
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

                }).ToList(),
                From = store.From,
                HoureFees = store.HoureFees,
                Id = store.Id,
                Matches = store.Matches.Select(mm => new ReturnMatchViewModel
                {
                    CityName = mm.City.Name,
                    StoreOwnerId = mm.Store.OwnerId,
                    CreatorId = mm.CreatorId,
                    CreatorName = mm.Creator.Name,
                    CreatorStoreName = mm.Creator.Stores.FirstOrDefault() == null ? null : mm.Creator.Stores.FirstOrDefault().StoreName,
                    StoreName = mm.Store.StoreName,
                    Date = mm.Date,
                    EntryFeeValue = mm.EntryFee.Value.Value,
                    Id = mm.Id,
                    NoOfSlots = mm.NofSlots.Value,
                    Prize = mm.Prize,
                    StoreAddress = mm.Store.Address,
                    StoreId = mm.StoreId.Value,
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
                }).ToList(),
                NumberOfDevices = store.NumberOfDevices,
                OpenTime = store.OpenTime,
                Photos = store.Photos,
                Role = UserRoles.StoreOwner,
                StoreName = store.StoreName,
                To = store.To,
                UserName = store.Owner.UserName,
                ValidUser = store.Owner.ValidUser
            };

            return Ok(resp);
        }

        // POST: api/Stores
        [ResponseType(typeof(Store))]
        public IHttpActionResult PostStore(StoreViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var appUser = db.Users.SingleOrDefault(u => u.Id == model.OwnerId);
            if (appUser == null)
            {
                return StatusCode(HttpStatusCode.MethodNotAllowed); //405
            }
            var store = new Store();
            store.Address = model.Address;
            
            store.CityId = model.CityId;
            store.CloseTime = model.CloseTime;
            store.From = model.From;
            store.HoureFees = model.HoureFees;
            store.NumberOfDevices = model.NumberOfDevices;
            store.OpenTime = model.OpenTime;
            store.OwnerId = model.OwnerId;
            store.StoreName = model.StoreName;
            store.To = model.To;
            store.Approved = true;

            store.WorkingHours = DateTime.Parse(model.CloseTime).Subtract(DateTime.Parse(model.OpenTime));
            
            db.Stores.Add(store);
            appUser.ValidUser = true;

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return NotFound(); //404
            }
            foreach (var item in model.Photos)
            {
                var storePhotos = new StorePhotos();
                storePhotos.Url = item;
                storePhotos.StoreId = store.Id;
                db.StorePhotos.Add(storePhotos);
            }
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                return NotFound(); //404
            }

            return Created(new Uri(Request.RequestUri + "/" + store.Id), store); //201
        }

        // DELETE: api/Stores/5
        [ResponseType(typeof(Store))]
        public IHttpActionResult DeleteStore(int id)
        {

            Store store = db.Stores.Find(id);
            if (store == null)
            {
                return NotFound();
            }
            
            store.Matches.ForEach(ss => ss.StoreId = null);
            var photos = db.StorePhotos.Where(ph => ph.StoreId == store.Id).ToList();
            db.StorePhotos.RemoveRange(photos);
            db.Stores.Remove(store);
            db.SaveChanges();

            return Ok(store);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StoreExists(int id)
        {
            return db.Stores.Count(e => e.Id == id) > 0;
        }
    }
}