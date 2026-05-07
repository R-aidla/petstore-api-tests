using System.Net;
using System.Text.Json;
using FluentAssertions;
using PetstoreApiTests.Helpers;
using PetstoreApiTests.Models;
using RestSharp;
using Xunit;

namespace PetstoreApiTests.Tests;

public class GetPetTests
{
    private readonly RestClient _client;

    public GetPetTests()
    {
        _client = new RestClient("https://petstore.swagger.io/v2");
    }

    [Fact]
    public async Task GetPet_ShouldReturnPet_WhenRequestIsValid()
    {
        long petId = TestDataFactory.TestID;
        await CRUDCommands.CreatePet(petId);

        try
        {
            var getRequest = new RestRequest($"/pet/{petId}", Method.Get);
            var response = await _client.ExecuteAsync(getRequest);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Should().NotBeNullOrEmpty();

            Pet? createdPet = JsonSerializer.Deserialize<Pet>(
                response.Content!,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            createdPet.Should().NotBeNull();

            createdPet!.Id.Should().Be(petId);
            createdPet.Name.Should().Be("Dragon");
            createdPet.Status.Should().Be("available");
        }
        finally
        {
            await CRUDCommands.DeletePet(petId);
        }
    }

    [Fact]
    public async Task GetPet_ShouldReturnNotFound_WhenRequestCantFindId()
    {
        long petId = 0;

        var getRequest = new RestRequest($"/pet/{petId}", Method.Get);
        var response = await _client.ExecuteAsync(getRequest);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetPet_ShouldReturnBadRequest_WhenRequestGivesInvalidId()
    {
        long petId = -1;

        var getRequest = new RestRequest($"/pet/{petId}", Method.Get);

        var response = await _client.ExecuteAsync(getRequest);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}