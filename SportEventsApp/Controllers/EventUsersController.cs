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

namespace SportEventsApp.Controllers
{
    
    public class EventUsersGetViewModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public int EventId { get; set; }
        public int? GroupId { get; set; }
        public bool Status { get; set; } = false;
        public string CashNumber { get; set; }

    }
    public class ReturnEventUsers
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public int EventId { get; set; }
        public bool Status { get; set; } = false;
        public string CashNumber { get; set; }
        public EventGroupVmodel Group { get; set; }
    }

    public class EventGroupVmodel
    {
        public int? GroupId { get; set; }
        public string GroupName { get; set; }
        public int EventId { get; set; }
        public List<SimplifiedUser> Users { get; set; }
    }

    public class EventUsersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        [HttpPost]
        [Route("api/GetEventsUsersByIds")]
        public IHttpActionResult GetEventsUsersByIds(EventUsersGetViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = db.EventUsers
                                .Include(eu => eu.Group)
                                .SingleOrDefault(eu => eu.EventId == model.EventId && eu.UserId == model.UserId);

            
            if (response == null)
            {
                return NotFound();
            }

            var users = db.EventUsers.Include(uu => uu.User).Where(gg => gg.GroupId == response.GroupId).Select(se => new SimplifiedUser
            {
                Id = se.User.Id,
                Name = se.User.Name,
                UserName = se.User.UserName,
                Mobile=se.User.Mobile

            }).ToList();

            var group = new EventGroupVmodel()
            {
                EventId = response.EventId,
                GroupId = response.GroupId,
                GroupName = response.Group == null ? null : response.Group.Name,
                Users = users
            };
            var Obj = new ReturnEventUsers
            {
                Status = response.Status,
                CashNumber = response.CashNumber,
                EventId = response.EventId,
                UserId = response.UserId,
                Group= group
            };
            return Ok(Obj);
        }

        [HttpPost]
        [Route("api/JoinTournament")]
        public IHttpActionResult JoinTournament(EventUsersGetViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // 400 
            }
            var dbmodel = db.EventUsers
                            .FirstOrDefault(eu => eu.EventId == model.EventId && eu.UserId == model.UserId);
            if (dbmodel != null)
            {
                return StatusCode(HttpStatusCode.Found); // 302
            }
                            
            var user = db.Users.SingleOrDefault(us => us.Id == model.UserId);
            if (user == null)
            {
                return NotFound(); //404
            }
            var response = new EventUsers();
            response.EventId = model.EventId;
            response.UserId = model.UserId;
            response.Status = false;

            var Prefix = user.Mobile.Substring(0, 3);
            if (Prefix.ToLower() == "010")
            {
                var VodafoneCashNumber = db.VodafoneCashs.Where(vo => vo.Event_ID == model.EventId && vo.Count < 12).FirstOrDefault();
                if (VodafoneCashNumber != null)
                {
                    VodafoneCashNumber.Count = VodafoneCashNumber.Count + 1;

                    response.CashNumber = VodafoneCashNumber.Number;

                    db.EventUsers.Add(response);

                }
                else
                {
                    //No number for payment
                    return StatusCode(HttpStatusCode.PaymentRequired); //402
                }


            }
            else if (Prefix.ToLower() == "011")
            {
                var EtisalatCashNumber = db.EtisalatCashs.Where(et => et.Event_ID == model.EventId && et.Count < 12).FirstOrDefault();
                if (EtisalatCashNumber != null)
                {
                    EtisalatCashNumber.Count = EtisalatCashNumber.Count + 1;
                    response.CashNumber = EtisalatCashNumber.Number;
                    db.EventUsers.Add(response);

                }
                else
                {
                    //No Number for payment
                    return StatusCode(HttpStatusCode.PaymentRequired); //402
                }

            }
            else
            {
                //not etisalat or vodaphone 
                return StatusCode(HttpStatusCode.PaymentRequired); //402
            }

            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                return StatusCode(HttpStatusCode.ExpectationFailed); //417
            }
            model.CashNumber = response.CashNumber;
            model.Status = response.Status;
            return Created(new Uri(Request.RequestUri + "/" + model.CashNumber), model); //201


        }

        [HttpPost]
        [Route("api/VerifyUserEvent")]
        public IHttpActionResult VerifyUserEvent(EventUsersGetViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var dbmodel = db.EventUsers.SingleOrDefault(eu => eu.EventId == model.EventId && eu.UserId == model.UserId);
            if (dbmodel == null)
            {
                return NotFound();
            }
            dbmodel.GroupId = model.GroupId;
            dbmodel.Status = model.Status;
            
            var eventModel = db.Events.SingleOrDefault(e => e.Id == model.EventId);
            var verifiedCount = db.EventUsers.Where(eu => eu.EventId == model.EventId).Count(eu=>eu.Status == true);
            if (verifiedCount >= eventModel.No_Of_Players)
            {
                eventModel.Full = true;

            }
            else
            {
                eventModel.Full = false;
            }

            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                return StatusCode(HttpStatusCode.ExpectationFailed); //417
            }
            model.Status = dbmodel.Status;
            return Ok(model);

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

    public class SimplifiedUser
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Mobile { get; set; }
    }
}