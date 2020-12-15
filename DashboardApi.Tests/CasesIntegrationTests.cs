using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace DashboardApi.Tests
{
    public class CasesIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public CasesIntegrationTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }
        
        
        [Theory]
        [InlineData("cases")]
        [InlineData("cases/topCountries/?sortby=confirmed_today")]
        [InlineData("cases/?country=Turkey")]
        [InlineData("cases/topCountries")]
        [InlineData("cases/totalCases")]
        [InlineData("cases/totalCases/?country=Turkey")]
        public async Task CasesControllerEndpointsReturnsSuccess(string url)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
        }
    }
}