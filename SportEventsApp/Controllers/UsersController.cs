//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Data.Entity.Infrastructure;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Web.Http;
//using System.Web.Http.Description;
//using SportEventsApp.Models;
//using System.Web.Http.Cors;

//namespace SportEventsApp.Controllers
//{
//    [EnableCors(origins: "*", headers: "*", methods: "*")]
//    public class UsersController : ApiController
//    {
//        private ApplicationDbContext db;
//        public UsersController()
//        {
//            db = new ApplicationDbContext();
//        }

//        // GET: api/Users
//        public IHttpActionResult GetEUsers()
//        {
//            var users = db.EUsers.Include(u => u.Group).Include(us => us.Event).ToList();
//            return Ok(users);
//        }

//        // GET: api/Users/5
//        public IHttpActionResult GetUser(int id)
//        {
//            User user = db.EUsers.Include(u => u.Group).Include(us => us.Event).SingleOrDefault(eu => eu.Id == id);
//            if (user == null)
//            {
//                return NotFound();
//            }
//            return Ok(user);
//        }
//        /// <summary>
//        /// Get Users Data by GroupID.
//        /// </summary>
//        /// <param name="id">The ID of the Group.</param>
//        [Route("api/GroupUsers/{id}")]
//        public IHttpActionResult GetUserByGroupId(int id)
//        {
//            var isExsist = db.Groups.Count(e => e.Id == id) > 0;
//            if (isExsist == false)
//            {
//                return BadRequest();
//            }
//            var users = db.EUsers.Include(u => u.Group).Include(us => us.Event).Where(g => g.Group_ID == id).ToList();
//            if (users == null) 
//            {
//                return NotFound();
//            }
//            return Ok(users);
//        }
//        // PUT: api/Users/5
//        public IHttpActionResult PutUser(int id, User user)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            if (id != user.Id)
//            {
//                return BadRequest();
//            }

//            var dbuser = db.EUsers.FirstOrDefault(u => u.Id == id);
//            dbuser.UserName = user.UserName;
//            dbuser.Mobile = user.Mobile;
//            dbuser.Status = user.Status;
//            dbuser.Group_ID = user.Group_ID;
//            dbuser.Event_ID = user.Event_ID;
//            dbuser.CashNumber = user.CashNumber;
//            try
//            {
//                db.SaveChanges();
               
//            }
//            catch (Exception e)
//            {
//                if (!UserExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    return StatusCode(HttpStatusCode.NotModified);
//                }
//            }
//            return Ok(user);

//        }

//        // POST: api/Users
//        public IHttpActionResult PostUser(User user)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }
//            var Prefix = user.Mobile.Substring(0, 3);
//            if (Prefix.ToLower() == "010")
//            {
//                var VodafoneCashNumber = db.VodafoneCashs.Where(vo => vo.Event_ID == user.Event_ID && vo.Count < 12).FirstOrDefault();
//                if (VodafoneCashNumber != null)
//                {
//                    VodafoneCashNumber.Count = VodafoneCashNumber.Count + 1;
//                    user.CashNumber = VodafoneCashNumber.Number;
//                    db.EUsers.Add(user);
//                    db.SaveChanges();
//                    return Created(new Uri(Request.RequestUri + "/" + user.Id), user);
//                }
//                else
//                {
//                    user.CashNumber = null;
//                    db.EUsers.Add(user);
//                    db.SaveChanges();
//                    return Created(new Uri(Request.RequestUri + "/" + user.Id), user);
//                }


//            }
//            else if (Prefix.ToLower() == "011")
//            {
//                var EtisalatCashNumber = db.EtisalatCashs.Where(et => et.Event_ID == user.Event_ID && et.Count < 12).FirstOrDefault();
//                if (EtisalatCashNumber != null)
//                {
//                    EtisalatCashNumber.Count = EtisalatCashNumber.Count + 1;
//                    user.CashNumber = EtisalatCashNumber.Number;
//                    db.EUsers.Add(user);
//                    db.SaveChanges();
//                    return Created(new Uri(Request.RequestUri + "/" + user.Id), user);
//                }
//                else
//                {
//                    user.CashNumber = null;
//                    db.EUsers.Add(user);
//                    db.SaveChanges();
//                    return Created(new Uri(Request.RequestUri + "/" + user.Id), user);
//                }

//            }
//            else
//            {
//                user.CashNumber = null;
//                db.EUsers.Add(user);
//                db.SaveChanges();
//                return Created(new Uri(Request.RequestUri + "/" + user.Id), user);
//            }

            

           
//        }

//        // DELETE: api/Users/5
//        public IHttpActionResult DeleteUser(int id)
//        {
//            User user = db.EUsers.Find(id);
//            if (user == null)
//            {
//                return NotFound();
//            }

//            db.EUsers.Remove(user);
//            db.SaveChanges();

//            return Ok(user);
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        private bool UserExists(int id)
//        {
//            return db.EUsers.Count(e => e.Id == id) > 0;
//        }
//    }
//}