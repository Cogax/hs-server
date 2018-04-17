using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using ShoppingListApi.Models;
using Xunit;

namespace ShoppingListApi.Test
{
    public class ShoppingListItemsTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public ShoppingListItemsTests() {
            _server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task GetAll_WhenCalled_ThenResponseIsValid()
        {
            // Act
            var response = await _client.GetAsync("/api/shoppinglistitem");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal("[{\"id\":1,\"label\":\"Banane\",\"isComplete\":false}]", responseString);
        }

        [Fact]
        public async Task GetById_WhenCalled_ThenResponseIsValid()
        {
            // Act
            var response = await _client.GetAsync("/api/shoppinglistitem/1");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal("{\"id\":1,\"label\":\"Banane\",\"isComplete\":false}", responseString);
        }

        [Fact]
        public async Task Create_WhenCalled_ThenResponseIsValid()
        {
            // Arrange
            var item = new ShoppingListItem { Label = "Apfel" };

            // Act
            var response = await _client.PostAsync("/api/shoppinglistitem", getJsonData(item));
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal("{\"id\":2,\"label\":\"Apfel\",\"isComplete\":false}", responseString);
            Assert.Equal("/api/ShoppingListItem/2", response.Headers.Location.LocalPath);
        }

        [Fact]
        public async Task Update_WhenCalled_ThenResponseIsValid()
        {
            // Arrange
            var data = new StringContent("{\"id\":1,\"label\":\"Banane\",\"isComplete\":true}", UnicodeEncoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/api/shoppinglistitem/1", data);
            response.EnsureSuccessStatusCode();

            response = await _client.GetAsync("/api/shoppinglistitem/1");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal("{\"id\":1,\"label\":\"Banane\",\"isComplete\":true}", responseString);
        }

        [Fact]
        public async Task Delete_WhenCalled_ThenResponseIsValid()
        {
            // Arrange
            var response = await _client.PostAsync("/api/shoppinglistitem", getJsonData(new ShoppingListItem { Label = "ToDelete" }));
            response.EnsureSuccessStatusCode();
            var item = await getObjectAsync(response);

            // Act
            response = await _client.DeleteAsync(string.Format("/api/shoppinglistitem/{0}", item.Id));
            response.EnsureSuccessStatusCode();
        }

        private StringContent getJsonData(object obj) {
            return new StringContent(JsonConvert.SerializeObject(obj), UnicodeEncoding.UTF8, "application/json");
        }

        private async Task<ShoppingListItem> getObjectAsync(HttpResponseMessage response)
        {
            var responseString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ShoppingListItem>(responseString);
        }
    }
}
