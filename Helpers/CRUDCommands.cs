using PetstoreApiTests.Models;
using RestSharp;
using System.Text.Json;

namespace PetstoreApiTests.Helpers
{
    public class CRUDCommands
    {
        private static readonly RestClient _client = new RestClient("https://petstore.swagger.io/v2");

        public static async Task<Pet?> CreatePet(long id)
        {
            Pet newPet = TestDataFactory.CreatePet(id);

            var createRequest = new RestRequest("/pet", Method.Post);
            createRequest.AddJsonBody(newPet);

            var response = await _client.ExecuteAsync(createRequest);

            Pet? createdPet = JsonSerializer.Deserialize<Pet>(
                response.Content!,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            return createdPet;
        }

        public static async Task<bool> DeletePet(long petId)
        {
            var request = new RestRequest($"/pet/{petId}", Method.Delete);

            var response = await _client.ExecuteAsync(request);

            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}
