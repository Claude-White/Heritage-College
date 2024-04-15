using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using W24HVKTEAM2.Models;

namespace W24HVKTEAM2.Controllers {

    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;

        private readonly HVKW24_Team2Context _context;

        public HomeController(ILogger<HomeController> logger, HVKW24_Team2Context _ctx) {
            _logger = logger;
            _context = _ctx;
        }

        [HttpGet]
        public IActionResult Index() {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Hvkuser newUser) {
            if (newUser.Email == null || newUser.Password == null) {
                ModelState.AddModelError("", "Please Enter A Valid Email And Password");
                return View(newUser);
            }

            var user = _context.Hvkusers
                 .FirstOrDefault(u => u.Email == newUser.Email && u.Password == newUser.Password);

            if (user != null) {
                HttpContext.Session.SetInt32("UserId", user.HvkuserId);
                if (user.UserType == "Customer") {
                    var userplusplus = _context.Hvkusers
                    .Include(u => u.Pets)
                    .ThenInclude(p => p.PetReservations)
                    .ThenInclude(pr => pr.Reservation)
                    .FirstOrDefault(u => u.Email == newUser.Email);
                    ViewBag.Pets = userplusplus.Pets;
                    return View("CustomerHome", userplusplus);
                } else {
                    var reseravtions = _context.Reservations
                        .Include(r => r.PetReservations)
                        .ThenInclude(pr => pr.Pet)
                        .ThenInclude(p => p.Hvkuser)
                        .OrderBy(r => r.StartDate)
                        .ThenBy(r => r.EndDate);
                    return View("StaffHome", reseravtions);
                }
            }

            ModelState.AddModelError("", "Account Does Not Exist");
            return View(newUser);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult StaffHome()
        {
            var userid = HttpContext.Session.GetInt32("UserId");

            var user = _context.Hvkusers.Find(userid);

            if (user != null && user.UserType == "Employee") {
                var reseravtions = _context.Reservations
                      .Include(r => r.PetReservations)
                      .ThenInclude(pr => pr.Pet)
                      .ThenInclude(p => p.Hvkuser)
                      .OrderBy(r => r.StartDate)
                      .ThenBy(r => r.EndDate);
                return View(reseravtions);
            }
            HttpContext.Session.Clear();
            return View("Index");
        }

        public IActionResult CustomerHome()
        {
            var userid = HttpContext.Session.GetInt32("UserId");

            var user = _context.Hvkusers.Find(userid);

            if (user != null && user.UserType == "Customer")
            {
                var userplusplus = _context.Hvkusers
                    .Include(u => u.Pets)
                    .ThenInclude(p => p.PetReservations)
                    .ThenInclude(pr => pr.Reservation)
                    .FirstOrDefault(u => u.HvkuserId == userid);
                ViewBag.Pets = userplusplus.Pets;

                return View(userplusplus);
            }
            HttpContext.Session.Clear();
            return View("Index");
        }

        public IActionResult Construction() {
            return View();
        }

    }
}