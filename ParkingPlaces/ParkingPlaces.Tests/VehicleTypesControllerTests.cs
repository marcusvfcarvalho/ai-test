using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using ParkingPlaces.Models;
using Xunit;

namespace ParkingPlaces.Tests;

public class VehicleTypesControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    public VehicleTypesControllerTests(WebApplicationFactory<Program> factory) => _client = factory.CreateClient();

    private static VehicleType Sample(string name = "Car", string desc = "Standard car", decimal price = 5.00m)
        => new() { Name = name, Description = desc, PricePerHour = price };

    [Fact] public async Task GetAll_ReturnsSuccess() { var r = await _client.GetAsync("/vehicletypes"); r.EnsureSuccessStatusCode(); }
    [Fact] public async Task GetById_NotFound() { var r = await _client.GetAsync("/vehicletypes/999"); Assert.Equal(HttpStatusCode.NotFound, r.StatusCode); }
    [Fact] public async Task Create_ReturnsCreated()
    {
        var r = await _client.PostAsJsonAsync("/vehicletypes", Sample("SUV", "Sport utility", 7.50m));
        Assert.Equal(HttpStatusCode.Created, r.StatusCode);
        var c = await r.Content.ReadFromJsonAsync<VehicleType>();
        Assert.NotNull(c); Assert.Equal("SUV", c.Name); Assert.Equal(7.50m, c.PricePerHour);
    }
    [Fact] public async Task GetById_Existing_ReturnsType()
    {
        var cr = await _client.PostAsJsonAsync("/vehicletypes", Sample("Bus", "Large bus", 15.00m));
        var c = await cr.Content.ReadFromJsonAsync<VehicleType>();
        var gr = await _client.GetAsync($"/vehicletypes/{c!.Id}"); gr.EnsureSuccessStatusCode();
        var f = await gr.Content.ReadFromJsonAsync<VehicleType>();
        Assert.NotNull(f); Assert.Equal("Bus", f.Name);
    }
    [Fact] public async Task Update_ReturnsUpdated()
    {
        var cr = await _client.PostAsJsonAsync("/vehicletypes", Sample());
        var c = await cr.Content.ReadFromJsonAsync<VehicleType>();
        var ur = await _client.PutAsJsonAsync($"/vehicletypes/{c!.Id}", Sample("Car Updated", "Updated", 6.00m));
        ur.EnsureSuccessStatusCode();
        var u = await ur.Content.ReadFromJsonAsync<VehicleType>();
        Assert.Equal("Car Updated", u!.Name); Assert.Equal(6.00m, u.PricePerHour);
    }
    [Fact] public async Task Update_NotFound() { var r = await _client.PutAsJsonAsync("/vehicletypes/999", Sample()); Assert.Equal(HttpStatusCode.NotFound, r.StatusCode); }
    [Fact] public async Task Delete_ReturnsNoContent()
    {
        var cr = await _client.PostAsJsonAsync("/vehicletypes", Sample());
        var c = await cr.Content.ReadFromJsonAsync<VehicleType>();
        var dr = await _client.DeleteAsync($"/vehicletypes/{c!.Id}");
        Assert.Equal(HttpStatusCode.NoContent, dr.StatusCode);
    }
    [Fact] public async Task Delete_NotFound() { var r = await _client.DeleteAsync("/vehicletypes/999"); Assert.Equal(HttpStatusCode.NotFound, r.StatusCode); }
    [Fact] public async Task Delete_RemovesFromList()
    {
        var cr = await _client.PostAsJsonAsync("/vehicletypes", Sample("Van", "Cargo van", 8.00m));
        var c = await cr.Content.ReadFromJsonAsync<VehicleType>();
        await _client.DeleteAsync($"/vehicletypes/{c!.Id}");
        var gr = await _client.GetAsync($"/vehicletypes/{c.Id}");
        Assert.Equal(HttpStatusCode.NotFound, gr.StatusCode);
    }
}
