using System.Net;
using System.Text.Json;
using FluentAssertions;
using PetstoreApiTests.Helpers;
using PetstoreApiTests.Models;
using RestSharp;
using Xunit;

namespace PetstoreApiTests.Tests;

public class DeletePetTests
{
    private readonly RestClient _client;

    public DeletePetTests()
    {
        _client = new RestClient("https://petstore.swagger.io/v2");
    }

    [Fact]
    public async Task DeletePet_ShouldReturnOK_WhenRequestIsValid()
    {
        long petId = TestDataFactory.TestID;
        Pet? createdPet = await CRUDCommands.CreatePet(petId);

        createdPet.Should().NotBeNull();


        var request = new RestRequest($"/pet/{petId}", Method.Delete);
        var response = await _client.ExecuteAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeletePet_ShouldReturnPetNotFound_WhenPetDoesntExist()
    {
        long petId = 0;

        var request = new RestRequest($"/pet/{petId}", Method.Delete);
        var response = await _client.ExecuteAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeletePet_ShouldReturnBadRequest_WhenIdIsNegative()
    {
        long petId = -1;

        var request = new RestRequest($"/pet/{petId}", Method.Delete);
        var response = await _client.ExecuteAsync(request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}