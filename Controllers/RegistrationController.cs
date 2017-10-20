using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Cseone.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace Cseone.Controllers
{
    public class RegistrationController : Controller
    {
        private CseoneContext _context;

        public RegistrationController(CseoneContext context)
        {
            _context = context;
        }

        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.Error = HttpContext.Session.GetString("Error");
            return View("Registration");
        }

        [HttpPost]
        [Route("Registration")]
        public IActionResult Registration(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                Console.WriteLine(model.FirstName);
                Console.WriteLine(model.LastName);
                Console.WriteLine(model.Email);
                Console.WriteLine(model.Password);

                User userExist = _context.Users.SingleOrDefault(x => x.Email == model.Email);

                if(userExist != null) {
                    HttpContext.Session.SetString("Error", String.Format("Email {0} already registered", model.Email));
                    return RedirectToAction("Index");
                }

                User user = new User {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = model.Password
                };

                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);

                _context.Users.Add(user);
                _context.SaveChanges();

                User newUser = _context.Users.SingleOrDefault(x => x.Email == model.Email);

                HttpContext.Session.SetString("FirstName",newUser.FirstName);
                HttpContext.Session.SetString("LastName", newUser.LastName);
                HttpContext.Session.SetString("Email", newUser.Email);
                HttpContext.Session.SetInt32("UserId", newUser.Id);
            }
            else {
                return View("Registration");
            }

            return RedirectToAction("Dashboard", "Dashboard");
        }


        [HttpPost]
        [Route("login")]
        public IActionResult Login(string email, string password)
        {
            if(!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(password)) {
                Console.WriteLine(email);
                Console.WriteLine(password);

                User user = _context.Users.SingleOrDefault(x => x.Email == email);

                if (user != null) {
                    HttpContext.Session.SetString("FirstName",user.FirstName);
                    HttpContext.Session.SetString("LastName", user.LastName);
                    HttpContext.Session.SetString("Email", user.Email);
                    HttpContext.Session.SetInt32("UserId", user.Id);

                    var Hasher = new PasswordHasher<User>();
                    // Pass the user object, the hashed password, and the PasswordToCheck
                    if(0 != Hasher.VerifyHashedPassword(user, user.Password, password))
                    {
                        //Handle success
                        return RedirectToAction("Dashboard", "Dashboard");
                    }
                    else {
                        HttpContext.Session.SetString("Error", String.Format("Password is not correct"));
                        return RedirectToAction("Index");
                    }
                }
                else {
                    HttpContext.Session.SetString("Error", String.Format("Email {0} is not registered", email));
                    return RedirectToAction("Index");
                }

            }

            return RedirectToAction("Dashboard", "Dashboard");
        }

        [HttpGet]
        [Route("logoff")]
        public IActionResult Logoff()
        {
                HttpContext.Session.Clear();
                return RedirectToAction("Index");
        }

    }
}
