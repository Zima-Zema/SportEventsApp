using SportEventsApp.Models;
using SportEventsApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportEventsApp.Controllers.Admin
{
    [Authorize]
    public class ApprovalController : Controller
    {
        ApplicationDbContext _context;
        public ApprovalController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Approval
        public ActionResult UserTournament()
        {
            var usertour = (from eu in _context.EventUsers
                            select new UserTournamentViewModel
                            {
                                CashNumber = eu.CashNumber,
                                GroupId=eu.GroupId,
                                EventId=eu.EventId,
                                UserId=eu.UserId,
                                Status = eu.Status,
                                Event = eu.Event,
                                Group=eu.Group,
                                User = eu.User,
                                Groups = _context.Groups.Where(gg => gg.Event_ID == eu.EventId).ToList()
                            }
                            ).ToList();

            return View(usertour);
        }


    }
}