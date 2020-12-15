using System.Threading.Tasks;
using DashboardApi.Controllers;
using DashboardApi.Models;
using DashboardApi.Tests.Services;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace DashboardApi.Tests
{
    public class AuthenticationControllerTests
    {
        private const string SecretKey = "some-random-secret-key-for-signing-jwt-tokens";

        
        // This type checks will be removed once we stop using dynamic dynamic objects on controllers
        [Fact]
        public async Task Register_ReturnsOkWithCorrectParameters()
        {
            var controller = new AuthController(new TestAuthService(SecretKey));
            var userToRegister = new UserDto {Email = "example@example.com", Password = "examplePassword"};
            
            var result = await controller.Register(userToRegister);

            var actionResult = Assert.IsType<OkObjectResult>(result);
            var actionValue = actionResult.Value;

            Assert.Equal(200, actionResult.StatusCode);
            var propertyInfo = actionValue.GetType().GetProperty("message");
            Assert.NotNull(propertyInfo);
            Assert.Equal("Successfully registered please login!", propertyInfo.GetValue(actionResult.Value, null));
        }


        [Fact]
        public async Task Register_ReturnsOkWithUnknownParameters()
        {
            var controller = new AuthController(new TestAuthService(SecretKey));
            var userToRegister = new UserDto { Email = "", Password = ""};

            var result = await controller.Register(userToRegister);

            var actionResult = Assert.IsType<OkObjectResult>(result);
            var actionValue = actionResult.Value;

            Assert.Equal(200, actionResult.StatusCode);
            var propertyInfo = actionValue.GetType().GetProperty("message");
            Assert.NotNull(propertyInfo);
            Assert.Equal("Successfully registered please login!", propertyInfo.GetValue(actionResult.Value, null));
        }

    }
}