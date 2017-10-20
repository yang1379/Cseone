using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Cseone.Models;
using System.Linq;
using System.Collections;
using Microsoft.EntityFrameworkCore;

namespace Cseone.Controllers
{
    public class DashboardController : Controller
    {
        private CseoneContext _context;

        public DashboardController(CseoneContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        [Route("dashboard")]
        public IActionResult Dashboard()
        {
            int? identifier = HttpContext.Session.GetInt32("UserId");

            if (identifier == null) {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Registration");
            }

            int userId = (int) identifier;

            List<Activity> activities = _context.Activities.Where(x => x.StartDate > DateTime.Now).OrderBy(x => x.StartDate).Include(x => x.Participants).ToList();

            string firstName = HttpContext.Session.GetString("FirstName");

            ViewBag.FirstName = firstName;
            ViewBag.UserId = userId;
            ViewBag.Activities = activities;
            return View("Dashboard");
        }

        [HttpGet]
        [Route("join/{activity_id}")]
        public IActionResult Join(int activity_id)
        {
            int? identifier = HttpContext.Session.GetInt32("UserId");

            if (identifier == null) {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Registration");
            }

            int userId = (int) identifier;

            Participant participant = new Participant {
                ActivityId = activity_id,
                UserId = userId
            };

            _context.Participants.Add(participant);
            _context.SaveChanges();
            
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        [Route("leave/{activity_id}")]
        public IActionResult Leave(int activity_id)
        {
            int? identifier = HttpContext.Session.GetInt32("UserId");

            if (identifier == null) {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Registration");
            }

            int userId = (int) identifier;

            Participant participant = _context.Participants.SingleOrDefault(x => (x.UserId == userId) && (x.ActivityId == activity_id));

            _context.Participants.Remove(participant);
            _context.SaveChanges();
            
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        [Route("delete/{activity_id}")]
        public IActionResult Delete(int activity_id)
        {
            int? identifier = HttpContext.Session.GetInt32("UserId");

            if (identifier == null) {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Registration");
            }

            int userId = (int) identifier;

            List<Participant> participants = _context.Participants.Where(x => x.ActivityId == activity_id).ToList();

            foreach(Participant guest in participants) {
                _context.Participants.Remove(guest);
            }

            Activity activity = _context.Activities.SingleOrDefault(x => x.Id == activity_id);
            _context.Activities.Remove(activity);

            _context.SaveChanges();
            
            return RedirectToAction("Dashboard");
        }

    }
}
