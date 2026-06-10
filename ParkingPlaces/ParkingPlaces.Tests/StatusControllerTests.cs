using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ParkingPlaces.Tests;

public class StatusControllerTests : IClassFixture<ParkingPlaceTests>
{
    private readonly HttpClient _client;

    public StatusControllerTests(ParkingPlaceTests factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_Status_ReturnsOk()
    {
        var response = await _client.GetAsync("/status");
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Get_Status_ReturnsJsonWithStatus()
    {
        var response = await _client.GetAsync("/status");
        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        var root = doc.RootElement;
        Assert.Equal("ok", root.GetProperty("status").GetString());
    }

    [Fact]
    public async Task Get_Status_ReturnsUptime()
    {
        var response = await _client.GetAsync("/status");
        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        var root = doc.RootElement;
        var uptime = root.GetProperty("uptime").GetString();
        Assert.NotNull(uptime);
        Assert.NotEmpty(uptime);
    }

    [Fact]
    public async Task Get_Status_ReturnsTimestamp()
    {
        var response = await _client.GetAsync("/status");
        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        var root = doc.RootElement;
        var timestamp = root.GetProperty("timestamp").GetString();
        Assert.NotNull(timestamp);
        Assert.True(DateTime.TryParse(timestamp, out _));
    }

    [Fact]
    public async Task Get_Status_ReturnsUptimeSeconds()
    {
        var response = await _client.GetAsync("/status");
        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);

        var root = doc.RootElement;
        var uptimeSeconds = root.GetProperty("uptimeSeconds").GetInt64();
        Assert.True(uptimeSeconds >= 0);
    }
}
