using System.Net;
using System.Text.Json;
using FluentAssertions;
using PetstoreApiTests.Helpers;
using PetstoreApiTests.Models;
using RestSharp;
using Xunit;

namespace PetstoreApiTests.Tests;

public class PutPetTests
{
    private readonly RestClient _client;

    public PutPetTests()
    {
        _client = new RestClient("https://petstore.swagger.io/v2");
    }

    [Fact]
    public async Task PutPet_ShouldReturnUpdatedPet_WhenRequestIsValid()
    {
        long petId = TestDataFactory.TestID;
        Pet? pet = await CRUDCommands.CreatePet(petId);

        pet.Should().NotBeNull();

        if (pet != null)
        {
            pet!.Name = "Drago";
            pet!.Status = "pending";
        }
        else
            return;

        try
        {
            var putRequest = new RestRequest("/pet", Method.Put);
            putRequest.AddJsonBody(pet!);

            var response = await _client.ExecuteAsync(putRequest);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            response.Content.Should().NotBeNullOrEmpty();

            Pet? updatedPet = JsonSerializer.Deserialize<Pet>(
                response.Content!,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            updatedPet.Should().NotBeNull();

            updatedPet!.Id.Should().Be(petId);
            updatedPet.Name.Should().Be("Drago");
            updatedPet.Status.Should().Be("pending");
        }
        finally
        {
            await CRUDCommands.DeletePet(petId);
        }
    }

    [Fact]
    public async Task PutPet_ShouldReturnNotFound_WhenRequestCantFindId()
    {
        long petId = 0;
        Pet invalidPet = TestDataFactory.CreatePet(petId, "Invalid");

        var putRequest = new RestRequest("/pet", Method.Put);
        putRequest.AddJsonBody(invalidPet);

        var response = await _client.ExecuteAsync(putRequest);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PutPet_ShouldReturnMethodNotAllowed_WhenRequestIsMissingPet()
    {
        long petId = TestDataFactory.TestID;
        Pet unexistentPet = new()
        {
            Id = petId,
            Name = null,
            Category = null,
            PhotoUrls = null,
            Status = "non-existing",
            Tags = null
        };

        var putRequest = new RestRequest("/pet", Method.Put);
        putRequest.AddJsonBody(unexistentPet);

        var response = await _client.ExecuteAsync(putRequest);

        response.StatusCode.Should().Be(HttpStatusCode.MethodNotAllowed);
    }
}