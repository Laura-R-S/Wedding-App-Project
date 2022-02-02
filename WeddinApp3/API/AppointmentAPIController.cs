using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WeddinApp3.Models.ViewModels;
using WeddinApp3.Services;
using WeddinApp3.Utility;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.Trainers;
using System.Linq.Expressions;
using WeddinApp3.Models;

namespace WeddinApp3.API
{

    [Route("api/Appointment")]
    [ApiController]
    public class AppointmentAPIController : Controller
    {
        //readonly can only be assigned a value from within the constructor of a class
        private readonly IAppointmentService _appointmentService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string loginId;
        private readonly string role;



        //constructor to create objects to use throughout the class 
        public AppointmentAPIController(IAppointmentService appointmentService, IHttpContextAccessor httpContextAccessor)
        {
            _appointmentService = appointmentService;
            _httpContextAccessor = httpContextAccessor;
            loginId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            role = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
        }

        //Method to retrieve status code and select output message to inform user
        //if booking has been added or updated 
        //calls AddUpdate method in AppointmentAPIController 
        [HttpPost]
        [Route("SaveCalendarData")]

        public IActionResult SaveCalendarData(AppointmentViewModel data)
        {
            //creating an object of class commonResponse  
            CommonResponse<int> commonResponse = new CommonResponse<int>();

            try
            {
                commonResponse.status = _appointmentService.AddUpdate(data, loginId).Result; 
                if(commonResponse.status == 1){
                    commonResponse.message = Helper.appointmentUpdated;
                }
                if (commonResponse.status == 2){
                    commonResponse.message = Helper.appointmentAdded;
                }
            } catch(Exception e)
            {
                commonResponse.message = e.Message;
                commonResponse.status = Helper.failure_code; 
            }
            
            return Ok(commonResponse); 
        }

        //Method to output booking details depending on role of user.
        //Supplier Id passed in to display bookings for admin
        [HttpGet]
        [Route("GetCalendarData")]

        public IActionResult GetCalendarData(string supplierId)
            {
            CommonResponse<List<AppointmentViewModel>> commonResponse = new CommonResponse<List<AppointmentViewModel>>();

            
            try
            {
                if (role == Helper.Couple)
                {
                    commonResponse.dataenum = _appointmentService.CoupleBookings(loginId);
                    commonResponse.status = Helper.success_code; 

                }else if (role == Helper.Supplier)
                {
                    commonResponse.dataenum = _appointmentService.SupplierBookings(loginId, role);
                    commonResponse.status = Helper.success_code;
                }else
                {
                    commonResponse.dataenum = _appointmentService.SupplierBookings(supplierId, role);
                    commonResponse.status = Helper.success_code;

                }
            }
            catch(Exception e)
            {
                commonResponse.message = e.Message;
                commonResponse.status = Helper.failure_code;
            }
            return Ok(commonResponse);
        }

        public ViewResult SaveCalendarData()
        {
            throw new NotImplementedException();
        }

        //Method to retrieve booking based on an ID 
        [HttpGet]
        [Route("GetCalendarDataById/{id}")]

        public IActionResult GetCalendarDataById(int id)
        {
            CommonResponse<AppointmentViewModel> commonResponse = new CommonResponse<AppointmentViewModel>();
            try
            {
                    commonResponse.dataenum = _appointmentService.GetById(id, role);
                    commonResponse.status = Helper.success_code;
            }
            catch (Exception e)
            {
                commonResponse.message = e.Message;
                commonResponse.status = Helper.failure_code;
            }
            return Ok(commonResponse);
        }

        //Method to delete booking 
        [HttpGet]
        [Route("DeleteAppointment/{id}")]

        public async Task<IActionResult> DeleteAppointment(int id)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();

            try
            {
                commonResponse.status = await _appointmentService.Delete(id);
                commonResponse.message = commonResponse.status == 1 ? Helper.appointmentDeleted : Helper.somethingWentWrong; 
            }
            catch (Exception e)
            {
                commonResponse.message = e.Message;
                commonResponse.status = Helper.failure_code;
            }
            return Ok(commonResponse);
        }

        //method to allow supplier to confirm appointments 
        [HttpGet]
        [Route("ConfirmEvent/{id}")]
        public IActionResult ConfirmEvent(int id)
        {
            CommonResponse<int> commonResponse = new CommonResponse<int>();

            try
            {
                var result = _appointmentService.ConfirmEvent(id).Result; 
                if(result > 0)
                {
                    commonResponse.status = Helper.success_code;
                    commonResponse.message = Helper.meetingConfirmed;
                }
                else
                {
                    commonResponse.status = Helper.failure_code; 
                    commonResponse.message = Helper.meetingConfirmedError;
                }
            }
            catch (Exception e)
            {
                commonResponse.message = e.Message;
                commonResponse.status = Helper.failure_code;
            }
            return Ok(commonResponse);
        }



    }
}
