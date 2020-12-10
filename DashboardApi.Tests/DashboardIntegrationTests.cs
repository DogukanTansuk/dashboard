using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using DashboardApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace DashboardApi.Tests
{
    public class DashboardIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public DashboardIntegrationTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }


        [Theory]
        [InlineData("/")]
        public async Task GetEndpointsSuccess(string url)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public void GetWelcomeReturnsWelcomeMessage()
        {
            var controller = new DashboardController(new NullLogger<DashboardController>());
            var result = controller.GetWelcome();
            
            var actionResult = Assert.IsType<ActionResult<Welcome>>(result);
            var returnValue = Assert.IsType<Welcome>(actionResult.Value);
            Assert.Equal("Welcome", returnValue.Message);
            
        }
    }
    
}