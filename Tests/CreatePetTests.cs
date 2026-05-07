using System.Net;
using System.Text.Json;
using FluentAssertions;
using PetstoreApiTests.Helpers;
using PetstoreApiTests.Models;
using RestSharp;
using Xunit;

namespace PetstoreApiTests.Tests;

public class CreatePetTests
{
    private readonly RestClient _client;

    public CreatePetTests()
    {
        _client = new RestClient("https://petstore.swagger.io/v2");
    }

    [Fact]
    public async Task CreatePet_ShouldReturnCreatedPet_WhenRequestIsValid()
    {
        long petId = TestDataFactory.TestID;
        Pet newPet = TestDataFactory.CreatePet(petId);

        try
        {
            var createRequest = new RestRequest("/pet", Method.Post);
            createRequest.AddJsonBody(newPet);

            var response = await _client.ExecuteAsync(createRequest);

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
    public async Task CreatePet_ShouldReturnMethodNotAllowed_WhenRequestMethodIsIncorrect()
    {
        long petId = TestDataFactory.TestID;
        Pet newPet = TestDataFactory.CreatePet(petId);

        var createRequest = new RestRequest("/pet", Method.Patch);
        createRequest.AddJsonBody(newPet);

        var response = await _client.ExecuteAsync(createRequest);

        response.StatusCode.Should().Be(HttpStatusCode.MethodNotAllowed);
    }

    [Fact]
    public async Task CreatePet_ShouldReturnCreatedPet_WhenRequestOnlyHasNameAndID()
    {
        long petId = TestDataFactory.TestID + 1;
        Pet newPet = TestDataFactory.CreatePet(petId, "MissingNo");

        newPet.Tags = null;
        newPet.Category = null;
        newPet.PhotoUrls = null;

        try
        {
            var createRequest = new RestRequest("/pet", Method.Post);
            createRequest.AddJsonBody(newPet);

            var response = await _client.ExecuteAsync(createRequest);

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
            createdPet.Name.Should().Be("MissingNo");
            createdPet.Status.Should().Be("available");
        }
        finally
        {
            await CRUDCommands.DeletePet(petId);
        }
    }
}