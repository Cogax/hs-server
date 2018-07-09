using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using FinanceApi.Models;
using Xunit;

namespace FinanceApi.Test
{
    public class ExpensesTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public ExpensesTests()
        {
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task GetAll_WhenCalled_ThenResponseIsValid()
        {
            // Act
            var response = await _client.GetAsync("/api/expense");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal("[{\"id\":1,\"label\":\"Banane\",\"amount\":100.0}]", responseString);
        }

        [Fact]
        public async Task GetById_WhenCalled_ThenResponseIsValid()
        {
            // Act
            var response = await _client.GetAsync("/api/expense/1");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal("{\"id\":1,\"label\":\"Banane\",\"amount\":100.0}", responseString);
        }

        [Fact]
        public async Task Create_WhenCalled_ThenResponseIsValid()
        {
            // Arrange
            var item = new Expense { Label = "Test", Amount = 55.55 };

            // Act
            var response = await _client.PostAsync("/api/expense", getJsonData(item));
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal("{\"id\":2,\"label\":\"Test\",\"amount\":55.55}", responseString);
            Assert.Equal("/api/Expense/2", response.Headers.Location.LocalPath);
        }
        private StringContent getJsonData(object obj)
        {
            return new StringContent(JsonConvert.SerializeObject(obj), UnicodeEncoding.UTF8, "application/json");
        }

        private async Task<Expense> getObjectAsync(HttpResponseMessage response)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Expense>(responseString);
        }
    }
}
