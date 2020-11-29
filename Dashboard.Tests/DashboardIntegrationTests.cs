using System;
using System.Threading.Tasks;
using DashboardApi;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DashboardApi.Tests
{
    public class DashboardIntegrationTests : IClassFixture<WebApplicationFactory<DashboardApi.Startup>>
    {
        private readonly WebApplicationFactory<DashboardApi.Startup> _factory;

        public DashboardIntegrationTests(WebApplicationFactory<DashboardApi.Startup> factory)
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