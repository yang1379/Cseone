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
    public class ActivityController : Controller
    {
        private CseoneContext _context;

        public ActivityController(CseoneContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("add_activity")]
        public IActionResult AddActivity()
        {
            int? identifier = HttpContext.Session.GetInt32("UserId");

            if (identifier == null) {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Registration");
            }

            return View("Activity");
        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(ActivityViewModel model)
        {
            int userId = (int) HttpContext.Session.GetInt32("UserId");

            string firstName = HttpContext.Session.GetString("FirstName");
            if(ModelState.IsValid)
            {
                int hour = model.ActivityTime.Hour;
                int minutes = model.ActivityTime.Minute;

                DateTime startTime = model.ActivityDate;
                TimeSpan ts = new TimeSpan(hour, minutes, 0);
                startTime = startTime.Date + ts;

                DateTime endTime = new DateTime();

                if(model.Units.ToString().Equals("Hours")) {
                    endTime = startTime.AddHours(model.Duration);
                }
                else if(model.Units.ToString().Equals("Minutes")) {
                    endTime = startTime.AddMinutes(model.Duration);
                }
                else {
                    endTime = startTime.AddDays(model.Duration);
                }

                Activity activity = new Activity {
                    Title = model.Title,
                    Coordinator = userId,
                    Coname = firstName,
                    Duration = model.Duration,
                    Unit = model.Units,
                    StartDate = startTime,
                    EndDate = endTime,
                    Description = model.Description
                    
                };

                _context.Activities.Add(activity);
                _context.SaveChanges();
            }
            else
            {
                return View("Activity");
            }

            Activity newActivity = _context.Activities.SingleOrDefault(x => x.Title == model.Title && x.Coordinator == userId);
            return RedirectToAction("ShowActivity", new {activity_id = newActivity.Id});
        }

        [HttpGet]
        [Route("show_activity/{activity_id}")]
        public IActionResult ShowActivity(int activity_id)
        {
            int? identifier = HttpContext.Session.GetInt32("UserId");

            if (identifier == null) {
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Registration");
            }

            int userId = (int) HttpContext.Session.GetInt32("UserId");

            Activity activity = _context.Activities.Where(x => x.Id == activity_id).Include(y => y.Participants).ThenInclude(z => z.User).FirstOrDefault();

            ViewBag.UserId = userId;
            ViewBag.Activity = activity;
            ViewBag.Coordinator = activity.Coname;
            return View("ShowActivity");
        }
        

    }
}
