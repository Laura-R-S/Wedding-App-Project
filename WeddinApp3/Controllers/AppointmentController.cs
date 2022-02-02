using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeddinApp3.Services;
using WeddinApp3.Utility;
using Microsoft.AspNetCore.Identity;
using WeddinApp3.Models;

namespace WeddinApp3.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        UserManager<ApplicationUser> _userManager;


        public AppointmentController(IAppointmentService appointmentService, UserManager<ApplicationUser> userManager)
        {
            _appointmentService = appointmentService;
            _userManager = userManager;
        }

        //Method to retrieve details from ViewBag and return View 
        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            ViewBag.SupplierList = _appointmentService.GetSupplierList(userId);
            ViewBag.CoupleList = _appointmentService.GetCoupleList();
            ViewBag.Duration = Helper.GetTimeDropDown();
            return View();
        }
    }
}
