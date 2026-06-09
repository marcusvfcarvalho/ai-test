using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using ParkingPlaces.Models;
using Xunit;

namespace ParkingPlaces.Tests;

public class CitiesControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    public CitiesControllerTests(WebApplicationFactory<Program> factory) => _client = factory.CreateClient();

    private static City Sample(string name = "Sao Paulo", string state = "SP", string country = "Brazil", int pop = 12325232)
        => new() { Name = name, State = state, Country = country, Population = pop };

    [Fact] public async Task GetAll_ReturnsSuccess() { var r = await _client.GetAsync("/cities"); r.EnsureSuccessStatusCode(); }
    [Fact] public async Task GetById_NotFound() { var r = await _client.GetAsync("/cities/999"); Assert.Equal(HttpStatusCode.NotFound, r.StatusCode); }
    [Fact] public async Task Create_ReturnsCreated()
    {
        var r = await _client.PostAsJsonAsync("/cities", Sample());
        Assert.Equal(HttpStatusCode.Created, r.StatusCode);
        var c = await r.Content.ReadFromJsonAsync<City>();
        Assert.NotNull(c); Assert.True(c.Id > 0);
    }
    [Fact] public async Task GetById_Existing_ReturnsCity()
    {
        var cr = await _client.PostAsJsonAsync("/cities", Sample("Rio", "RJ", "Brazil", 6748000));
        var c = await cr.Content.ReadFromJsonAsync<City>();
        var gr = await _client.GetAsync($"/cities/{c!.Id}"); gr.EnsureSuccessStatusCode();
        var f = await gr.Content.ReadFromJsonAsync<City>();
        Assert.NotNull(f); Assert.Equal("Rio", f.Name);
    }
    [Fact] public async Task Update_ReturnsUpdated()
    {
        var cr = await _client.PostAsJsonAsync("/cities", Sample());
        var c = await cr.Content.ReadFromJsonAsync<City>();
        var ur = await _client.PutAsJsonAsync($"/cities/{c!.Id}", Sample("Updated", "SP", "Brazil", 13000000));
        ur.EnsureSuccessStatusCode();
        var u = await ur.Content.ReadFromJsonAsync<City>();
        Assert.Equal("Updated", u!.Name);
    }
    [Fact] public async Task Update_NotFound() { var r = await _client.PutAsJsonAsync("/cities/999", Sample()); Assert.Equal(HttpStatusCode.NotFound, r.StatusCode); }
    [Fact] public async Task Delete_ReturnsNoContent()
    {
        var cr = await _client.PostAsJsonAsync("/cities", Sample());
        var c = await cr.Content.ReadFromJsonAsync<City>();
        var dr = await _client.DeleteAsync($"/cities/{c!.Id}");
        Assert.Equal(HttpStatusCode.NoContent, dr.StatusCode);
    }
    [Fact] public async Task Delete_NotFound() { var r = await _client.DeleteAsync("/cities/999"); Assert.Equal(HttpStatusCode.NotFound, r.StatusCode); }
    [Fact] public async Task Delete_RemovesFromList()
    {
        var cr = await _client.PostAsJsonAsync("/cities", Sample("BH", "MG", "Brazil", 2523000));
        var c = await cr.Content.ReadFromJsonAsync<City>();
        await _client.DeleteAsync($"/cities/{c!.Id}");
        var gr = await _client.GetAsync($"/cities/{c.Id}");
        Assert.Equal(HttpStatusCode.NotFound, gr.StatusCode);
    }
}
