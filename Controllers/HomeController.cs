using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CsharpBelt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CsharpBelt.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbcontext;
        public static PasswordHasher<User> RegisterHasher = new PasswordHasher<User>();
        public static PasswordHasher<LoginUser> LoginHasher = new PasswordHasher<LoginUser>();

        public HomeController(MyContext context)
        {
            dbcontext = context;
        }

        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            return Redirect("signin");
        }

        [HttpGet("signin")]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(User u)
        {
            User userInDb = dbcontext.GetUserByEmail(u.Email);
            if (userInDb != null)
            {
                ModelState.AddModelError("Email", "Email already exists");
            }
            if (ModelState.IsValid)
            {
                u.Password = RegisterHasher.HashPassword(u, u.Password);
                int UserId = dbcontext.Create(u);
                HttpContext.Session.SetInt32("UserId", UserId);
                return Redirect("home");
            }
            return View("signin");
        }

       
        [HttpPost("login")]
        public IActionResult Login(LoginUser u)
        {
            User userInDb = dbcontext.GetUserByEmail(u.LoginEmail);
            if (userInDb == null)
            {
                ModelState.AddModelError("LoginEmail", "Email doesn't exist");
            }
            if (ModelState.IsValid)
            {
                var result = LoginHasher.VerifyHashedPassword(u, userInDb.Password, u.LoginPassword);
                if (result == 0)
                {
                    ModelState.AddModelError("LoginPassword", "Try again");
                }
                else
                {
                    HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                    return Redirect("home");
                }
            }
            return View("signin");
        }

        [HttpGet("home")]
        public IActionResult Home(Activityy a)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if (UserId == null)
            {
                return Redirect("signin");
            }
            else
            {
                ViewBag.User = dbcontext.GetUserById((int)UserId);
                ViewBag.Users = dbcontext.Users.ToList();
                ViewBag.Parts = dbcontext.Participants.Include(act => act.User).ToList();
                ViewBag.Activities = dbcontext.Activities
                .OrderBy(d => d.Date)
                .Include(act => act.EventInfo)
                .ToList();
                return View();
            }
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }

        
        [HttpGet("new")]
        public IActionResult NewActivity()
        {
            return View();
        }

        [Route("addactivity")]
        [HttpPost]
        public IActionResult CreateEvent(Activityy a)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if(ModelState.IsValid)
            {
                a.CoordinatorId = (int)UserId;
                dbcontext.Create(a);
                return Redirect("home");
            }
            return Redirect("new");
            
            
        }

        [HttpGet("join/{ActivityId}")]
        public IActionResult Join(int ActivityId)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            Participant p = new Participant();
            p.ActivityId = ActivityId;
            p.UserId = (int)UserId;
            dbcontext.Create(p);
            return RedirectToAction("Home");
        }

        [HttpPost("delete/{ActivityId}")]
        public IActionResult Delete(int ActivityId)
        {
            dbcontext.Remove(ActivityId);
            return RedirectToAction("Home");
        }

        [Route("leave/{ActivityId}")]
        [HttpGet]
        public IActionResult Leave(int ActivityId)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            dbcontext.Remove(ActivityId, (int)UserId);
            return RedirectToAction("Home");
        }

        [HttpGet("activity/{ActivityId}")]
        public IActionResult Details(int ActivityId)
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if (UserId == null)
            {
                return Redirect("signin");
            }

            ViewBag.Activities = dbcontext.Activities.Include(act => act.EventInfo).ToList();
            ViewBag.User = dbcontext.GetUserById((int)UserId);
            ViewBag.Users = dbcontext.Users.ToList();
            ViewBag.num = dbcontext.GetActivityById(ActivityId).EventInfo.ToList();
            return View(dbcontext.GetActivityById(ActivityId));
        }

        



    }
}
