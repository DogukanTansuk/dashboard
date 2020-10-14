using System;
using System.Threading.Tasks;
using Dashboard;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Dashboard.Tests
{
    public class DashboardIntegrationTests : IClassFixture<WebApplicationFactory<Dashboard.Startup>>
    {
        private readonly WebApplicationFactory<Dashboard.Startup> _factory;

        public DashboardIntegrationTests(WebApplicationFactory<Dashboard.Startup> factory)
        {
            _factory = factory;
        }


        [Theory]
        [InlineData("/weatherforecast")]
        public async Task GetEndpointsSuccess(string url)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
        }
    }
}