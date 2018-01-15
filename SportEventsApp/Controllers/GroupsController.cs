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
    public class GroupsController : ApiController
    {
        private ApplicationDbContext db;
        public GroupsController()
        {
            db = new ApplicationDbContext();
        }

        /// <summary>
        /// Get List Of Groups In Including Event for each Group.
        /// </summary>
        // GET: api/Groups
        public IHttpActionResult GetGroups()
        {
            var Groups = db.Groups.Include(g => g.Event).ToList();
            return Ok(Groups);
        }

        // GET: api/Groups/5
        /// <summary>
        /// Get Group including event and List of users by ID.
        /// </summary>
        /// <param name="id">The ID of the Group.</param>
        public IHttpActionResult GetGroup(int id)
        {
            Group group = db.Groups.Include(g => g.Event).Include(gg=>gg.Users).SingleOrDefault(gr => gr.Id == id);
            if (group == null)
            {
                return NotFound();
            }

            return Ok(group);
        }
        /// <summary>
        /// Get Group Data by UserID.
        /// </summary>
        /// <param name="userId">The ID of the User.</param>
        [Route("api/UserGroup/{userId}")]
        public IHttpActionResult GetGroupByUserId(int userId)
        {
            var isExsist = db.EUsers.Count(e => e.Id == userId) > 0;
            if (isExsist == false)
            {
                return BadRequest();
            }
            Group group = db.EUsers.SingleOrDefault(uu => uu.Id == userId).Group;
            if (group == null)
            {
                return NotFound();
            }

            return Ok(group);
        }
        /// <summary>
        /// Get list of Groups by EventId.
        /// </summary>
        /// <param name="eventId">The ID of the Event.</param>
        [Route("api/EventGroups/{eventId}")]
        public IHttpActionResult GetGroupsByEventId(int eventId)
        {
            var isExsist = db.Events.Count(e => e.Id == eventId) > 0;
            if (isExsist == false)
            {
                return BadRequest();
            }
            var groups = db.Events.SingleOrDefault(ee => ee.Id == eventId).Groups;
            if (groups == null || groups.Count == 0)
            {
                return NotFound();
            }

            return Ok(groups);
        }
        
        // PUT: api/Groups/5
        /// <summary>
        /// Update Group by Group id
        /// </summary>
        /// <param name="id">The ID of the Group.</param>
        /// <param name="group">Group Object From Request body.</param>
        public IHttpActionResult PutGroup(int id, Group group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != group.Id)
            {
                return BadRequest();
            }

            db.Entry(group).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(HttpStatusCode.NotModified);
                }
            }

            return Ok(group);
        }

        // POST: api/Groups
        /// <summary>
        /// Add group by method post
        /// </summary>
        /// <param name="group">Group Object From Request body.</param>
        public IHttpActionResult PostGroup(Group group)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Groups.Add(group);
            db.SaveChanges();

            return Created(new Uri(Request.RequestUri + "/" + group.Id), group);
        }

        // DELETE: api/Groups/5
        /// <summary>
        /// Delete Group by Group id
        /// </summary>
        /// <param name="id">The ID of the Group.</param>
        public IHttpActionResult DeleteGroup(int id)
        {
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return NotFound();
            }

            db.Groups.Remove(group);
            db.SaveChanges();

            return Ok(group);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GroupExists(int id)
        {
            return db.Groups.Count(e => e.Id == id) > 0;
        }
    }
}