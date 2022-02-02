using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeddinApp3.Utility
{
    public static class Helper
    {
        public static string Admin = "Admin";
        public static string Couple = "Couple";
        public static string Supplier = "Supplier";

        public static string appointmentAdded = "Appointment Added Successfully";
        public static string appointmentUpdated = "Appointment Updated Successfully";
        public static string appointmentDeleted = "Appointment Deleted Successfully";
        public static string appointmentExists = "Appointment for selected date and time already exists";
        public static string appointmentNotExist = "Appointment does not exist";
        public static string meetingConfirmed = "Meeting Confirmed Successfully";
        public static string meetingConfirmedError = "Something went wrong, please try again";

        public static string appointmentAddError = "Something went wrong, please try again";
        public static string appointmentUpdateError = "Something went wrong, please try again";
        public static string somethingWentWrong = "Something went wrong, please try again";

        public static int success_code = 1;
        public static int failure_code = 0;

        //Method to return List of drop down for Login. 
        //Couple and Supplier only visible couple/supplier users and admin drop down only visible to admin users 
        public static List<SelectListItem> GetRolesForDropDown(bool AdminLogdIn)
        {
            if (AdminLogdIn)
            {
                return new List<SelectListItem>
            {
                new SelectListItem{Value=Helper.Admin, Text=Helper.Admin},
            };

            }
            else
            {
                return new List<SelectListItem>
            {
                new SelectListItem{Value=Helper.Couple, Text=Helper.Couple},
                new SelectListItem{Value=Helper.Supplier, Text=Helper.Supplier}
            };
            }
        }

        //Duration drop down in booking pop up modal 
        public static List<SelectListItem> GetTimeDropDown()
        {
            int min = 60;

            List<SelectListItem> duration = new List<SelectListItem>();

            for (int i = 0; i <= 12; i++)
            {
                duration.Add(new SelectListItem { Value = min.ToString(), Text = i + " Hour" });
                min = min + 30;
                duration.Add(new SelectListItem { Value = min.ToString(), Text = i + " Hour 30 minutes" });
                min = min + 30;
            }

            return duration;
        }
    }
}
