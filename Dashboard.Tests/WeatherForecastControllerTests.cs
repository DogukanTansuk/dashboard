using System;
using System.Linq;
using Xunit;
using Moq;
using DashboardApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DashboardApi.Tests
{
    public class WeatherForecastControllerTests
    {
        [Fact]
        public void GetReturnsCorrectNumberOfElements()
        {
            var logger = Mock.Of<ILogger<WeatherForecastController>>();
            var controller = new WeatherForecastController(logger);
            var result = controller.Get();
            Assert.Equal(5, result.Count());
        }
    }
}
