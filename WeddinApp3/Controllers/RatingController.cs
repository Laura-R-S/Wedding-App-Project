using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeddinApp3.Models;

namespace WeddinApp3.Views.Ratings
{
    public class RatingController : Controller
    {

        private readonly ApplicationDbContext _db;
        UserManager<ApplicationUser> _userManager;

        public RatingController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            var userID = _userManager.GetUserId(User);

            if (userID == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = (from u in _db.Venues
                        where u.UserID == userID
                        select u);

            if (user.Count() > 0)
            {
                //redirect them to the appointment page
                //uncomment this to see the survey
                //return RedirectToAction("index", "Appointment");
            }

            //IEnumerable<Rating> objList = _db.Ratings;
            return View();
        }

        public IActionResult AddRatings()
        {
            return View();
        }

        // method to add wedding survey ratngs to Venue database  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddRatings(Models.ViewModels.SurveyViewModel obj) 
        {
            var userID = _userManager.GetUserId(User);
            var num = 0;
            var venues = new List<string>()
                    {
                        "b80fefe4-935e-46be-98fc-9060db4c2f6f",
                        "ae28bf15-358b-44b5-8d9e-1a308a1fa9ad",
                        "546198fa-f46a-4a71-9203-2faf6f9f0b08",
                        "62105bf1-3290-45fc-9520-1840f210ad46",
                        "9cbcfaef-0695-49fe-a9fa-316ebf2cac2d"
                    };

            foreach (var property in obj.GetType().GetProperties())
            {
                var prop = property.GetValue(obj, null);
                var score = Int32.Parse(prop.ToString());

                var surveyEntry = new Venue
                {
                    UserID = userID,
                    VenID = venues[num],
                    Score = score
                };

                _db.Venues.Add(surveyEntry);
                _db.SaveChanges();

                num += 1;
            }

            return RedirectToAction("index", "Appointment");
        }
    }
}
