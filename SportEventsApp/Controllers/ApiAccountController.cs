using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using SportEventsApp.Models;
using SportEventsApp.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Data.Entity;
using System.Web.Http.Cors;

namespace SportEventsApp.Controllers
{

    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ApiAccountController : ApiController
    {
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;
        private ApplicationDbContext _context;
        public ApiAccountController()
        {
            _context = new ApplicationDbContext();
        }
        public ApiAccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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





        [HttpPost]
        [AllowAnonymous]
        [Route("api/Register")]
        public async Task<IHttpActionResult> Register([FromBody]RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var isUser = await UserManager.FindByNameAsync(model.UserName);
            if (isUser != null)
            {
                return StatusCode(HttpStatusCode.Found);
            }
            var user = new ApplicationUser { UserName = model.UserName, Email = model.UserName + "@Fantasista.com", Mobile = model.UserName,ValidUser=false };

            var result = await UserManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                //temp
                var roleStore = new RoleStore<IdentityRole>(new ApplicationDbContext());
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                if (!roleManager.RoleExists(model.Role))
                {
                    var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                    role.Name = model.Role;
                    roleManager.Create(role);
                }
                await UserManager.AddToRoleAsync(user.Id, model.Role);

                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                var response = new UserReturn
                {
                    Id = user.Id,
                    Name = user.Name,
                    UserName = user.UserName,
                    Mobile = user.Mobile,
                    ValidUser=user.ValidUser,
                    Role = model.Role
                };
                return Ok(response);
            }
            return NotFound();

        }
        [HttpPost]
        [AllowAnonymous]
        [Route("api/Login")]
        public async Task<IHttpActionResult> Login([FromBody]LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    var appUser = _context.Users
                        .Include(cc=>cc.City)
                        .Include(ss=>ss.Stores).Where(u => u.UserName == model.UserName).FirstOrDefault();
                    var R = await UserManager.GetRolesAsync(appUser.Id);

                    if (R.FirstOrDefault() == UserRoles.Player)
                    {
                        var events = appUser.EventUsers
                            .Where(uu => uu.UserId == appUser.Id)
                            .Select(ww => new EventWithStatus { Event = new ReturnEventViewModel
                            {
                                End = ww.Event.End,
                                Id = ww.Event.Id,
                                EntryFees = ww.Event.Entry_Fees,
                                From = ww.Event.From,
                                Full = ww.Event.Full,
                                MatchDuration = ww.Event.MatchDuration,
                                Name = ww.Event.Name,
                                NoOfPlayers = ww.Event.No_Of_Players,
                                Prize1 = ww.Event.Prize_1,
                                Prize2 = ww.Event.Prize_2,
                                Prize3 = ww.Event.Prize_3,
                                Published = ww.Event.Published,
                                Start = ww.Event.Start,
                                To = ww.Event.To,
                                Type = ww.Event.Type,
                                Stores = ww.Event.Stores.Select(s => new ReturnEventStores
                                {
                                    storeOwnerId = s.OwnerId,
                                    cityName = s.City.Name,
                                    storeName = s.StoreName
                                }).ToList()
                            }, Status = ww.Status, CashNumber = ww.CashNumber }).ToList();

                        var matchs = appUser.Matches.Select(mm => new ReturnMatchViewModel
                        {
                            CityName = mm.City.Name,
                            StoreOwnerId=mm.Store.OwnerId,
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

                        }).ToList();


                        var response = new UserReturn
                        {
                            Id = appUser.Id,
                            Name = appUser.Name,
                            UserName = appUser.UserName,
                            CityID = appUser.City_ID,
                            CityName = appUser.City == null ? null : appUser.City.Name,
                            Mobile = appUser.Mobile,
                            Address = appUser.Address,
                            PictureUrl = appUser.PictureUrl,
                            Matches = matchs,
                            //Groups = appUser.Groups,
                            Events = events,
                            ValidUser = appUser.ValidUser,
                            Role = R.FirstOrDefault()
                        };
                        return Ok(response);
                    }
                    if (R.FirstOrDefault()==UserRoles.StoreOwner)
                    {
                        if (appUser.Stores.Count>0)
                        {
                            var storeID = appUser.Stores.FirstOrDefault().Id;
                            Store store = _context.Stores
                                            .Include(ss=>ss.Photos)
                                            .Include(s => s.Owner)
                                            .Include(s => s.Matches)
                                            .Include(e => e.Events)
                                            .Include(ss => ss.City)
                                            .SingleOrDefault(st => st.Id == storeID);
                            var Owner = new UserReturn()
                            {
                                Id = store.Owner.Id,
                                Name = store.Owner.Name,
                                Address = store.Owner.Address,
                                Mobile = store.Owner.Mobile,
                                PictureUrl = store.Owner.PictureUrl,
                                Role = R.FirstOrDefault(),
                                UserName = store.Owner.UserName

                            };
                            var storematches = store.Matches.Select(mm => new ReturnMatchViewModel
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

                            }).ToList();

                            var eve = store.Events.Select(e => new ReturnEventViewModel
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
                            var storeResponse = new StoreReturn()
                            {
                                Address = store.Address,
                                City = store.City,
                                Approved = store.Approved,
                                Id = store.Id,
                                CloseTime = store.CloseTime,
                                Events = eve,
                                From = store.From,
                                HoureFees = store.HoureFees,
                                Matches = storematches,
                                NumberOfDevices = store.NumberOfDevices,
                                OpenTime = store.OpenTime,
                                Owner = Owner,
                                Photos = store.Photos,
                                Role = R.FirstOrDefault(),
                                StoreName = store.StoreName,
                                To = store.To,
                                UserName = appUser.UserName,
                                ValidUser = appUser.ValidUser

                            };
                            return Ok(storeResponse);
                        }
                        else
                        {
                            return Json(new
                            {
                                id=appUser.Id,
                                userName = appUser.UserName,
                                validUser = appUser.ValidUser,
                                role = R.FirstOrDefault()
                            });
                        }

                    }
                    else
                    {
                        return StatusCode(HttpStatusCode.MethodNotAllowed);
                    }

                    
                case SignInStatus.LockedOut:
                    return StatusCode(HttpStatusCode.Unauthorized);
                case SignInStatus.Failure:
                default:
                    return NotFound();
            }
        }
        [HttpPut]
        [AllowAnonymous]
        [Route("api/UpdateProfile/{id}")]
        public async Task<IHttpActionResult> UpdateProfile([FromUri]string id,[FromBody] UserReturn model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != model.Id)
            {
                return BadRequest();
            }
            
            var appUser = await UserManager.FindByIdAsync(model.Id);
            if (appUser == null)
            {
                return NotFound();
            }
            appUser.Name = model.Name;
            if (model.UserName != null)
            {
                appUser.UserName = appUser.Mobile = model.UserName;
            }
            appUser.City_ID = model.CityID;
            appUser.Address = model.Address;
            appUser.PictureUrl = model.PictureUrl;
            appUser.ValidUser = true;

            var res = await UserManager.UpdateAsync(appUser);

            if (res.Errors.Contains($"Name {model.UserName} is already taken."))
            {
                return Conflict();
            }
            if (!res.Succeeded)
            {
                return StatusCode(HttpStatusCode.NotModified);
            }
            try
            {
                int c = _context.SaveChanges();
            }
            catch (Exception e)
            {

                return StatusCode(HttpStatusCode.NotModified);
            }
            var R = await UserManager.GetRolesAsync(appUser.Id);
            var events = appUser.EventUsers.Where(uu => uu.UserId == appUser.Id).Select(ww => new EventWithStatus { Event = new ReturnEventViewModel {
                End=ww.Event.End,
                Id=ww.Event.Id,
                EntryFees=ww.Event.Entry_Fees,
                From=ww.Event.From,
                Full=ww.Event.Full,
                MatchDuration=ww.Event.MatchDuration,
                Name=ww.Event.Name,
                NoOfPlayers= ww.Event.No_Of_Players,
                Prize1=ww.Event.Prize_1,
                Prize2=ww.Event.Prize_2,
                Prize3 = ww.Event.Prize_3,
                Published=ww.Event.Published,
                Start=ww.Event.Start,
                To=ww.Event.To,
                Type=ww.Event.Type,
                Stores= ww.Event.Stores.Select(s => new ReturnEventStores
                {
                    storeOwnerId = s.OwnerId,
                    cityName = s.City.Name,
                    storeName = s.StoreName
                }).ToList()
            }, Status = ww.Status,CashNumber=ww.CashNumber })
                .ToList();
            var matches = appUser.Matches.Select(mm => new ReturnMatchViewModel
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

            }).ToList();
            var response = new UserReturn
            {
                Id = appUser.Id,
                Name = appUser.Name,
                UserName = appUser.UserName,
                CityID = appUser.City_ID,
                CityName = appUser.City == null ? null : appUser.City.Name,
                Mobile = appUser.Mobile,
                Address = appUser.Address,
                PictureUrl = appUser.PictureUrl,
                Matches = matches,
                //Groups = appUser.Groups,
                Events = events,
                ValidUser = appUser.ValidUser,
                Role = R.FirstOrDefault()
            };
            return Ok(response);
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("api/GetUserById/{id}")]
        public async Task<IHttpActionResult> GetUserById([FromUri] string id)
        {

            var appUser = _context.Users
                .Include(cc=>cc.City)
                .Include(uu=>uu.Stores)
                .Include(uu=>uu.Matches)
                .SingleOrDefault(u => u.Id == id);
            if (appUser == null)
            {
                return NotFound();
            }
            var R = await UserManager.GetRolesAsync(appUser.Id);


            var appUsreMatches = appUser.Matches.Select(mm => new ReturnMatchViewModel
            {
                CityName = mm.City.Name,
                CreatorId = mm.CreatorId,
                StoreOwnerId = mm.Store.OwnerId,
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

            }).ToList();



            if (R.FirstOrDefault() == UserRoles.Player)
            {
                var events = appUser.EventUsers
                    .Where(uu => uu.UserId == appUser.Id)
                    .Select(ww => new EventWithStatus { Event = new ReturnEventViewModel
                    {
                        End = ww.Event.End,
                        Id = ww.Event.Id,
                        EntryFees = ww.Event.Entry_Fees,
                        From = ww.Event.From,
                        Full = ww.Event.Full,
                        MatchDuration = ww.Event.MatchDuration,
                        Name = ww.Event.Name,
                        NoOfPlayers = ww.Event.No_Of_Players,
                        Prize1 = ww.Event.Prize_1,
                        Prize2 = ww.Event.Prize_2,
                        Prize3 = ww.Event.Prize_3,
                        Published = ww.Event.Published,
                        Start = ww.Event.Start,
                        To = ww.Event.To,
                        Type = ww.Event.Type,
                        Stores = ww.Event.Stores.Select(s => new ReturnEventStores
                        {
                            storeOwnerId = s.OwnerId,
                            cityName = s.City.Name,
                            storeName = s.StoreName
                        }).ToList()
                    }, Status = ww.Status, CashNumber = ww.CashNumber } ).ToList();
                
                //var matches = appUser.Matches.Select(mm => new ReturnMatchViewModel
                //{
                //    CityName = mm.City.Name,
                //    CreatorId = mm.CreatorId,
                //    StoreOwnerId = mm.Store.OwnerId,
                //    CreatorName = mm.Creator.Name,
                //    CreatorStoreName = mm.Creator.Stores.FirstOrDefault() == null ? null : mm.Creator.Stores.FirstOrDefault().StoreName,
                //    StoreName = mm.Store.StoreName,
                //    Date = mm.Date,
                //    EntryFeeValue = mm.EntryFee.Value.Value,
                //    Id = mm.Id,
                //    NoOfSlots = mm.NofSlots.Value,
                //    Prize = mm.Prize,
                //    StoreAddress = mm.Store.Address,
                //    StoreId = mm.StoreId.Value,
                //    Time = mm.Time,
                //    Type = mm.Type,
                //    Users = mm.Users.Select(uu => new MatchUsersViewModel
                //    {
                //        Id = uu.Id,
                //        Mobile = uu.Mobile,
                //        Name = uu.Name,
                //        StoreName = uu.Stores.FirstOrDefault() == null ? null : uu.Stores.FirstOrDefault().StoreName,
                //        UserName = uu.UserName
                //    }).ToList()

                //}).ToList();

                
                var response = new UserReturn
                {
                    Id = appUser.Id,
                    Name = appUser.Name,
                    UserName = appUser.UserName,
                    CityID = appUser.City_ID,
                    CityName = appUser.City == null ? null : appUser.City.Name,
                    Mobile = appUser.Mobile,
                    Address = appUser.Address,
                    PictureUrl = appUser.PictureUrl,
                    Matches = appUsreMatches,
                    Events = events,
                    ValidUser = appUser.ValidUser,
                    Role = R.FirstOrDefault()
                };
                return Ok(response);
            }
            if (R.FirstOrDefault() == UserRoles.StoreOwner)
            {
                if (appUser.Stores.Count > 0)
                {
                    var storeID = appUser.Stores.FirstOrDefault().Id;
                    Store store = _context.Stores
                                    .Include(ss => ss.Photos)
                                    .Include(s => s.Owner)
                                    .Include(s => s.Matches)
                                    .Include(e => e.Events)
                                    .Include(ss => ss.City)
                                    .SingleOrDefault(st => st.Id == storeID);
                    var Owner = new UserReturn()
                    {
                        Id = store.Owner.Id,
                        Name = store.Owner.Name,
                        Address = store.Owner.Address,
                        Mobile = store.Owner.Mobile,
                        PictureUrl = store.Owner.PictureUrl,
                        Role = R.FirstOrDefault(),
                        UserName = store.Owner.UserName

                    };
                    //var storematches = store.Matches.Select(mm => new ReturnMatchViewModel
                    //{
                    //    CityName = mm.City.Name,
                    //    CreatorId = mm.CreatorId,
                    //    StoreOwnerId = mm.Store.OwnerId,
                    //    CreatorName = mm.Creator.Name,
                    //    CreatorStoreName = mm.Creator.Stores.FirstOrDefault() == null ? null : mm.Creator.Stores.FirstOrDefault().StoreName,
                    //    StoreName = mm.Store.StoreName,
                    //    Date = mm.Date,
                    //    EntryFeeValue = mm.EntryFee.Value.Value,
                    //    Id = mm.Id,
                    //    NoOfSlots = mm.NofSlots.Value,
                    //    Prize = mm.Prize,
                    //    StoreAddress = mm.Store.Address,
                    //    StoreId = mm.StoreId.Value,
                    //    Time = mm.Time,
                    //    Type = mm.Type,
                    //    Users = mm.Users.Select(uu => new MatchUsersViewModel
                    //    {
                    //        Id = uu.Id,
                    //        Mobile = uu.Mobile,
                    //        Name = uu.Name,
                    //        StoreName = uu.Stores.FirstOrDefault() == null ? null : uu.Stores.FirstOrDefault().StoreName,
                    //        UserName = uu.UserName
                    //    }).ToList()

                    //}).ToList();


                    



                    var storeResponse = new StoreReturn()
                    {
                        Address = store.Address,
                        City = store.City,
                        Approved = store.Approved,
                        Id = store.Id,
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
                        Matches = appUsreMatches,
                        NumberOfDevices = store.NumberOfDevices,
                        OpenTime = store.OpenTime,
                        Owner = Owner,
                        Photos = store.Photos,
                        Role = R.FirstOrDefault(),
                        StoreName = store.StoreName,
                        To = store.To,
                        UserName = appUser.UserName,
                        ValidUser = appUser.ValidUser,


                    };
                    return Ok(storeResponse);

                }
                else
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
            }
            else
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("api/GetInfoAdmin")]
        public IHttpActionResult GetInfoAdmin()
        {
            var regular = _context.Infos.Where(i => i.NestedId == null).Select(i => new InfoViewModel
            {
                Id = i.Id,
                Title = i.Title,
                Type = InfoType.Regular
            }).ToList();

            var nested = _context.NestedInfos.Select(i => new InfoViewModel
            {
                Id = i.Id,
                Title = i.Header,
                Type = InfoType.Nested
            }).ToList();

            var response = new List<InfoViewModel>();
            response.AddRange(regular);
            response.AddRange(nested);
            return Ok(response);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("api/GetInfo")]
        public IHttpActionResult GetInfo()
        {
            var regular = _context.Infos.Where(i=>i.NestedId == null).Select(i => new InfoApiViewModel
            {
                Id = i.Id,
                Title = i.Title,
                Points = i.Points.Select(pp => pp.Value).ToList(),
                Type = InfoType.Regular
            }).ToList();

            var nested = _context.NestedInfos.Select(i => new InfoApiViewModel
            {
                Id = i.Id,
                Title = i.Header,
                InfoList = i.InfoList.Select(rr => new RegularInfoViewModel
                {
                    Id = rr.Id,
                    Title = rr.Title,
                    Points = rr.Points.Select(pp => pp.Value).ToList()
                }).ToList(),
                Type = InfoType.Nested
            }).ToList();

            var response = new List<InfoApiViewModel>();
            response.AddRange(regular);
            response.AddRange(nested);
            return Ok(response);
        }


        [HttpDelete]
        [AllowAnonymous]
        [Route("api/DeleteInfo/{id}")]
        public IHttpActionResult DeleteInfo(int id)
        {
            var obj = _context.Infos.SingleOrDefault(i => i.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            obj.Points.Clear();
            var points = _context.Points.Where(p => p.InfoId == obj.Id).ToList();
            _context.Points.RemoveRange(points);
            _context.Infos.Remove(obj);
            _context.SaveChanges();
            return Ok(obj);
        }
        [HttpDelete]
        [AllowAnonymous]
        [Route("api/DeleteNested/{id}")]
        public IHttpActionResult DeleteNested(int id)
        {
            
            var obj = _context.NestedInfos.SingleOrDefault(co=>co.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            obj.InfoList.Clear();
            var infoList = _context.Infos.Where(p => p.NestedId == obj.Id).ToList();
            foreach (var item in infoList)
            {
                _context.Points.RemoveRange(item.Points);
            }
            _context.Infos.RemoveRange(infoList);
            _context.NestedInfos.Remove(obj);
            _context.SaveChanges();
            return Ok(obj);

        }




        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
        private bool UserExists(string id)
        {
            return _context.Users.Count(e => e.Id == id) > 0;
        }

    }
}
