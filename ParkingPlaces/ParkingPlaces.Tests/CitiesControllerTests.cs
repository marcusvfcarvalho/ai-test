using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using ParkingPlaces.Models;
using Xunit;

namespace ParkingPlaces.Tests;

public class CitiesControllerTests : IClassFixture<ParkingPlaceTests>
{
    private readonly HttpClient _client;

    public CitiesControllerTests(ParkingPlaceTests factory)
    {
        _client = factory.CreateClient();
    }

    private static City CreateSampleCity(string name = "Sao Paulo", string state = "SP", string country = "Brazil", int population = 12325232)
    {
        return new City { Name = name, State = state, Country = country, Population = population };
    }

    [Fact]
    public async Task GetAll_ReturnsSuccessStatusCode()
    {
        var response = await _client.GetAsync("/cities");
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task GetById_NonExistent_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/cities/999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateCity_ReturnsCreatedWithCity()
    {
        var city = CreateSampleCity();
        var response = await _client.PostAsJsonAsync("/cities", city);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var created = await response.Content.ReadFromJsonAsync<City>();
        Assert.NotNull(created);
        Assert.Equal(city.Name, created.Name);
        Assert.True(created.Id > 0);
    }

    [Fact]
    public async Task GetById_ExistingCity_ReturnsCity()
    {
        var city = CreateSampleCity("Rio de Janeiro", "RJ", "Brazil", 6748000);
        var createResponse = await _client.PostAsJsonAsync("/cities", city);
        var created = await createResponse.Content.ReadFromJsonAsync<City>();
        var getResponse = await _client.GetAsync($"/cities/{created!.Id}");
        getResponse.EnsureSuccessStatusCode();
        var fetched = await getResponse.Content.ReadFromJsonAsync<City>();
        Assert.NotNull(fetched);
        Assert.Equal("Rio de Janeiro", fetched.Name);
    }

    [Fact]
    public async Task UpdateCity_ReturnsUpdatedCity()
    {
        var city = CreateSampleCity();
        var createResponse = await _client.PostAsJsonAsync("/cities", city);
        var created = await createResponse.Content.ReadFromJsonAsync<City>();
        var updatedCity = new City { Name = "Sao Paulo Updated", State = "SP", Country = "Brazil", Population = 13000000 };
        var updateResponse = await _client.PutAsJsonAsync($"/cities/{created!.Id}", updatedCity);
        updateResponse.EnsureSuccessStatusCode();
        var result = await updateResponse.Content.ReadFromJsonAsync<City>();
        Assert.NotNull(result);
        Assert.Equal("Sao Paulo Updated", result.Name);
    }

    [Fact]
    public async Task UpdateCity_NonExistent_ReturnsNotFound()
    {
        var city = CreateSampleCity();
        var response = await _client.PutAsJsonAsync("/cities/999", city);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteCity_ReturnsNoContent()
    {
        var city = CreateSampleCity();
        var createResponse = await _client.PostAsJsonAsync("/cities", city);
        var created = await createResponse.Content.ReadFromJsonAsync<City>();
        var deleteResponse = await _client.DeleteAsync($"/cities/{created!.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteCity_NonExistent_ReturnsNotFound()
    {
        var response = await _client.DeleteAsync("/cities/999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteCity_RemovesFromList()
    {
        var city = CreateSampleCity("Belo Horizonte", "MG", "Brazil", 2523000);
        var createResponse = await _client.PostAsJsonAsync("/cities", city);
        var created = await createResponse.Content.ReadFromJsonAsync<City>();
        await _client.DeleteAsync($"/cities/{created!.Id}");
        var getResponse = await _client.GetAsync($"/cities/{created.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }
}
