/*using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using WeddinApp3.API;
using WeddinApp3.Models;
using WeddinApp3.Models.ViewModels;
using WeddinApp3.Views.Ratings;
using Xunit;
using WeddinApp3.Controllers;


namespace TestSayIDoApp
{


    public class TestingControllerMethods
    {


        private HomeController _homeController;

        private AccountController _accountController;
        private RatingController _ratingController;
        private AppointmentController _appointmentController;
        private AppointmentAPIController _appointmentAPIController;

        [Fact]
        public void IndexViewReturnedInHomeContoller()
        {
            //Arrange 
            _homeController = new HomeController();

            //Act 
            var result = _homeController.Index() as ViewResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void LoginViewReturnedInAccountController()
        {
            //Arrange 
            _accountController = new AccountController();

            //Act 
            var result = _accountController.Login() as ViewResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void LogOffMethodInAccountController()
        {
            //Arrange 
            _accountController = new AccountController();

            //Act 
            var result = _accountController.LogOff();

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void IndexViewReturnedInRatingContoller()
        {
            //Arrange 
            _ratingController = new RatingController();

            //Act 
            var result = _ratingController.Index();

            //Assert
            Assert.Equal("Index", (IEnumerable<char>)result);
        }

        [Fact]
        public void AddRatingsViewReturnedInRatingContoller()
        {
            //Arrange 
            _ratingController = new RatingController();

            //Act 
            var result = _ratingController.AddRatings() as ViewResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void IndexViewReturnedInAppointmentContoller()
        {
            //Arrange 
            _appointmentController = new AppointmentController();

            //Act 
            var result = _appointmentController.Index() as ViewResult;

            //Assert
            Assert.NotNull(result);
        }


        [Fact]
        public void IndexViewReturnedInAppointmentAPIContoller()
        {
            //Arrange 
            _appointmentAPIController = new AppointmentAPIController();

            //Act 
            var result = _appointmentAPIController.SaveCalendarData() as ViewResult;

            //Assert
            Assert.NotNull(result);
        }


    }
}
*/