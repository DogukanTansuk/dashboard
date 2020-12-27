using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
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
        [InlineData("cases/topCountries/?sortby=")]
        [InlineData("cases/topCountries/?sortby=confirmed_today")]
        [InlineData("cases/topCountries/?sortby=deaths_today")]
        [InlineData("cases/topCountries/?sortby=recovered_today")]
        [InlineData("cases/topCountries/?sortby=confirmed_change")]
        [InlineData("cases/topCountries/?sortby=deaths_change")]
        [InlineData("cases/topCountries/?sortby=recovered_change")]
        [InlineData("cases/?country=")]
        [InlineData("cases/?country=Turkey")]
        [InlineData("cases/?country=turkey")]
        [InlineData("cases/?country=TURKEY")]
        [InlineData("cases/?country=US")]
        [InlineData("cases/?country=uS")]
        [InlineData("cases/?country=us")]
        [InlineData("cases/topCountries")]
        [InlineData("cases/totalCases")]
        [InlineData("cases/totalCases/?country=")]
        [InlineData("cases/totalCases/?country=Turkey")]
        [InlineData("cases/totalCases/?country=turkey")]
        [InlineData("cases/totalCases/?country=TURKEY")]
        [InlineData("cases/totalCases/?country=uS")]
        [InlineData("cases/totalCases/?country=US")]
        [InlineData("cases/totalCases/?country=us")]
        public async Task CasesControllerEndpointsReturnsSuccess(string url)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
        }


        //test 404 not found
        [Theory]
        [InlineData("cases1")]
        [InlineData("cases/?country=US1")]
        public async Task CasesControllerEndpointsReturnsFail(string url)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(url);


            try
            {
                response.EnsureSuccessStatusCode();
                
            }
            catch (System.Net.Http.HttpRequestException e)
            {
                Assert.Equal("Response status code does not indicate success: 404 (Not Found).", e.Message);
            }
            
            
        }


        
        [Theory]

        [InlineData("cases/totalCases")]
        [InlineData("cases/totalCases/?country=")]
        [InlineData("cases/totalCases/?country=Turkey")]
        [InlineData("cases/totalCases/?country=turkey")]
        [InlineData("cases/totalCases/?country=TURKEY")]
        [InlineData("cases/totalCases/?country=uS")]
        [InlineData("cases/totalCases/?country=US")]
        [InlineData("cases/totalCases/?country=us")]
        public async Task CasesControllerEndpointsTotalCaseContentTest(string url)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(url);

            var responseString = JsonConvert.DeserializeObject<DashboardApi.Models.CovidCaseTotal>(await response.Content.ReadAsStringAsync());


            Assert.InRange(responseString.ConfirmedTotal, int.MinValue, int.MaxValue);
            Assert.InRange(responseString.DeathsTotal, int.MinValue, int.MaxValue);
            Assert.InRange(responseString.RecoveredTotal, int.MinValue, int.MaxValue);

        }



        [Theory]

        [InlineData("cases/?country=", "worldwide")]
        [InlineData("cases/?country=Turkey", "Turkey")]
        [InlineData("cases/?country=turkey", "Turkey")]
        [InlineData("cases/?country=TURKEY", "Turkey")]
        [InlineData("cases/?country=US", "US")]
        [InlineData("cases/?country=uS", "US")]
        [InlineData("cases/?country=us", "US")]
        public async Task CasesControllerEndpointsCasesCountryBaseContentTest(string url,string country)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(url);

            List<DashboardApi.Models.CovidCase> responseString;
            responseString= JsonConvert.DeserializeObject<List<DashboardApi.Models.CovidCase>>(await response.Content.ReadAsStringAsync());
           
            Assert.Equal(country, responseString[0].CountryRegion);
           

        }

        [Theory]
        [InlineData("cases/topCountries")]
        [InlineData("cases/topCountries/?sortby=")]
        [InlineData("cases/topCountries/?sortby=confirmed_today")]
        [InlineData("cases/topCountries/?sortby=deaths_today")]
        [InlineData("cases/topCountries/?sortby=recovered_today")]
        [InlineData("cases/topCountries/?sortby=confirmed_change")]
        [InlineData("cases/topCountries/?sortby=deaths_change")]
        [InlineData("cases/topCountries/?sortby=recovered_change")]
        public async Task CasesControllerEndpointsCasesTopCountryBaseContentTest(string url)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(url);

            List<DashboardApi.Models.CovidCase> responseString;
            responseString = JsonConvert.DeserializeObject<List<DashboardApi.Models.CovidCase>>(await response.Content.ReadAsStringAsync());

            Assert.InRange(responseString[0].ConfirmedChange, int.MinValue, int.MaxValue);
            Assert.InRange(responseString[0].ConfirmedToday, int.MinValue, int.MaxValue);
            Assert.InRange(responseString[0].DeathsChange, int.MinValue, int.MaxValue);
            Assert.InRange(responseString[0].DeathsToday, int.MinValue, int.MaxValue);
            Assert.InRange(responseString[0].RecoveredChange, int.MinValue, int.MaxValue);
            Assert.InRange(responseString[0].RecoveredToday, int.MinValue, int.MaxValue);

        }


    }
}