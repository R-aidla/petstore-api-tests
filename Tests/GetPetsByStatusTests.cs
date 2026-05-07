using System.Net;
using System.Text.Json;
using FluentAssertions;
using PetstoreApiTests.Helpers;
using PetstoreApiTests.Models;
using RestSharp;
using Xunit;

namespace PetstoreApiTests.Tests;

public class GetPetsByStatusTests
{
    private readonly RestClient _client;

    public GetPetsByStatusTests()
    {
        _client = new RestClient("https://petstore.swagger.io/v2");
    }

    [Fact]
    public async Task GetPetsByStatus_ShouldReturnAListOfAvailablePets_WhenRequestQueryCheckForAvailableString()
    {
        var getRequest = new RestRequest("/pet/findByStatus?status=available", Method.Get);
        var response = await _client.ExecuteAsync(getRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Should().NotBeNullOrEmpty();

        List<Pet>? pets = JsonSerializer.Deserialize<List<Pet>>(
            response.Content!,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        pets.Should().NotBeNull();
        
        if(pets != null)
            pets!.Count.Should().BeGreaterThanOrEqualTo(1);
    }

    [Fact]
    public async Task GetPetsByStatus_ShouldReturnBadRequest_WhenRequestContainsInvalidQueryString()
    {
        var getRequest = new RestRequest("/pet/findByStatus?status=invalid", Method.Get);
        var response = await _client.ExecuteAsync(getRequest);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetPetsByStatus_ShouldReturnBadRequest_WhenRequestIsMissingQuery()
    {
        var getRequest = new RestRequest("/pet/findByStatus", Method.Get);
        var response = await _client.ExecuteAsync(getRequest);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}