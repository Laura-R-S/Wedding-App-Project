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
using System.Threading.Tasks;
using WeddinApp3.Services;
using Moq;

namespace TestSayIDoApp
{
    class UnitTest2
    {

        public class AppointmentControllerTests
        {

            [Fact]
            public void ReturnsBadRequestResult_WhenModelStateIsInvalid()
            {
                // Arrange
                var mockService = new Mock<IAppointmentService>();
                object p = mockService.Setup(service => service.AddUpdate(GetTestSessions)); //.Returns(Task.FromResult(GetTestSessions()));
                var controller = new AppointmentController(mockService.Object);
                controller.ModelState.AddModelError("SessionName", "Required");
                //var newSession = new AppointmentController.NewSessionModel();

                // Act
                var result = controller.Index();

                // Assert
                var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.IsType<SerializableError>(badRequestResult.Value);
            }

            private List<TestViewModel> GetTestSessions()
            {
                var sessions = new List<TestViewModel>();
                sessions.Add(new TestViewModel()
                {
                    Id = "1",
                    Name = "Test One"
                });
                sessions.Add(new TestViewModel()
                {
                    Id = "2",
                    Name = "Test Two"
                });
                return sessions;
            }
        }


    }
}*/
