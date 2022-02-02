using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeddinApp3.Models;
using WeddinApp3.Models.ViewModels;
using WeddinApp3.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using WeddinApp3ML.Model;

namespace WeddinApp3.Services
{
    public class AppointmentService : IAppointmentService
    {

        private readonly ApplicationDbContext _db;
        private readonly IEmailSender _emailSender;
        UserManager<ApplicationUser> _userManager;

        public AppointmentService()
        {
        }

        public AppointmentService(ApplicationDbContext db, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _db = db;
            _emailSender = emailSender; 


        }

        //Metod using the appointmentViewModel to retrieve booking details 
        //returns statsus code to SaveCalendarData method 
        public async Task<int> AddUpdate(AppointmentViewModel model, string loginId)
        {
            if (model.CoupleId == null)
            {
                model.CoupleId = loginId;
            }

            var startDate = DateTime.Parse(model.StartDate);
            var endDate = DateTime.Parse(model.StartDate).AddMinutes(Convert.ToDouble(model.Duration));
            var couple = _db.Users.FirstOrDefault(u => u.Id == model.CoupleId);
            var supplier = _db.Users.FirstOrDefault(u => u.Id == model.SupplierId);

            if (model != null && model.Id > 0)
            {
                //update
                var Entity = _db.Appointments.FirstOrDefault(item => item.Id == model.Id);

                Entity.Title = model.Title;
                Entity.Description = model.Description;
                Entity.StartDate = startDate;
                Entity.EndDate = endDate;
                Entity.Duration = model.Duration;
                Entity.CoupleId = model.CoupleId;

                _db.Appointments.Update(Entity);
                await _db.SaveChangesAsync();
                return 1; 
            }
            else
            {
                //create 
                Appointment appointment = new Appointment()
                {
                    Title = model.Title,
                    Description = model.Description,
                    StartDate = startDate,
                    EndDate = endDate,
                    Duration = model.Duration,
                    SupplierId = model.SupplierId,
                    CoupleId = model.CoupleId,
                    SupplierApprovedApt = false,
                    AdminId = model.AdminId

                };

                await _emailSender.SendEmailAsync(supplier.Email, "Wedding Booking Requested",
                    $"Booking made by {couple.Name} is pending");
                await _emailSender.SendEmailAsync(couple.Email, "Wedding Booking Request sent",
                    $"Booking made for {supplier.Name} is pending");
                _db.Appointments.Add(appointment);
                await _db.SaveChangesAsync();
                return 2; 
}
        }
         
        //Booking details saved to database if approved by supplier 
        public async Task<int> ConfirmEvent(int id)
        {
            var appointment = _db.Appointments.FirstOrDefault(x => x.Id == id);
            if(appointment != null)
            {
                appointment.SupplierApprovedApt = true;
                return await _db.SaveChangesAsync();   
            }
            return 0; 
        }
        
        //method to retrieve couple booings from the database and return list of couple booking to GetCalendarData Method 
        public List<AppointmentViewModel> CoupleBookings(string coupleId)
        {
            return _db.Appointments.Where(x => x.CoupleId == coupleId).ToList().Select(y => new AppointmentViewModel()
            {
                Id = y.Id,
                Description = y.Description,
                StartDate = y.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = y.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Title = y.Title,
                Duration = y.Duration,
                SupplierApprovedApt = y.SupplierApprovedApt,
                Role = "couple"
            }).ToList();
        }

        //method to locate the first instance of Id passed in the database and
        //remove method applied to delete from the database and changes saved
        public async Task<int> Delete(int id)
        {
            var appointment = _db.Appointments.FirstOrDefault(x => x.Id == id);
            if (appointment != null)
            {
                _db.Appointments.Remove(appointment);
                return await _db.SaveChangesAsync();
            }
            return 0;
        }



        //method used in the GetCalendarById method to return a single user booking
        public AppointmentViewModel GetById(int id, string role)
        {
            return _db.Appointments.Where(x => x.Id == id).ToList().Select(y => new AppointmentViewModel()
            {
                Id = y.Id,
                Description = y.Description,
                StartDate = y.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = y.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Title = y.Title,
                Duration = y.Duration,
                SupplierApprovedApt = y.SupplierApprovedApt,
                CoupleId=y.CoupleId,
                SupplierId=y.SupplierId,
                SupplierName = _db.Users.Where(x => x.Id == y.SupplierId).Select(x => x.Name).FirstOrDefault(),
                CoupleName =_db.Users.Where(x=>x.Id==y.CoupleId).Select(x => x.Name).FirstOrDefault(),
                Role = role
            }).SingleOrDefault();
        }

        //method to return couple list retrieved from the database 
        public List<CoupleViewModel> GetCoupleList()
        {
            var couple = (from user in _db.Users
                          join userRoles in _db.UserRoles on user.Id equals userRoles.UserId
                          join roles in _db.Roles.Where(x => x.Name == Helper.Couple) on userRoles.RoleId equals roles.Id
                          //projection to only retrieve certain columns 
                          select new CoupleViewModel

                          {
                              Id = user.Id,
                              Name = user.Name
                          }
                          ).ToList();

            return couple;
        }

        //return list of suppliers and list of venues with prediction score 
        public List<SupplierViewModel> GetSupplierList(string userId)
        {

            var venueIds = new List<string>()
                    {
                        "b80fefe4-935e-46be-98fc-9060db4c2f6f",
                        "ae28bf15-358b-44b5-8d9e-1a308a1fa9ad",
                        "546198fa-f46a-4a71-9203-2faf6f9f0b08",
                        "62105bf1-3290-45fc-9520-1840f210ad46",
                        "9cbcfaef-0695-49fe-a9fa-316ebf2cac2d"
                    };

            List<float> results = new List<float>();

            // loop over the properties of the object
            foreach (var venId in venueIds)
            {
                var input = new ModelInput()
                {
                    VenId = venId,
                    UserID = userId
                };

                // Load model and predict output of sample data
                ModelOutput result = ConsumeModel.Predict(input);
                results.Add(result.Score);
            }

            var position = results.IndexOf(results.Max());
            var recVenue = venueIds[position];

            var suppliers = (from user in _db.Users
                             join userRoles in _db.UserRoles on user.Id equals userRoles.UserId
                             join roles in _db.Roles.Where(x => x.Name == Helper.Supplier) on userRoles.RoleId equals roles.Id
                             //projection to only retrieve certain columns
                             select new SupplierViewModel

                             {
                                 Id = user.Id,
                                 Name = user.Name + (user.Id == recVenue ? " (recommended)" : "")

                             }
              ).ToList();

            return suppliers;
        }

        //method to return list of supplier booking retrieved from database 
        public List<AppointmentViewModel> SupplierBookings(string supplierId, string role)
        {
            return _db.Appointments.Where(x => x.SupplierId == supplierId).ToList().Select(y => new AppointmentViewModel()
            {
                Id = y.Id,
                Description = y.Description,
                StartDate = y.StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = y.EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Title = y.Title,
                Duration = y.Duration,
                SupplierApprovedApt = y.SupplierApprovedApt,
                Role = role

            }).ToList();
        }
    }
}
