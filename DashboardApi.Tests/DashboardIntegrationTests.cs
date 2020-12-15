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


        [Theory]
        [InlineData("cases")]
        public async Task GetCasesSuccess(string url)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData("cases/?country=Turkey")]
        public async Task GetCasesWithCountrySuccess(string url)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
        }


        [Theory]
        [InlineData("cases/topCountries")]
        public async Task GetTopCountriesSuccess(string url)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData("cases/topCountries/?sortby=confirmed_today")]
        public async Task GetTopCountriesWithSortBySuccess(string url)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData("cases/totalCases")]
        public async Task GetTotalCasesSuccess(string url)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
        }

        [Theory]
        [InlineData("cases/totalCases/?country=Turkey")]
        public async Task GetTotalCasesWithCountrySuccess(string url)
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