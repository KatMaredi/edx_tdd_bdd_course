using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using practicing_tdd.controllers;
using practicing_tdd.data;
using practicing_tdd.models;

namespace practicing_tdd.tests;

public class CounterTests
{
    private AppDbContext _context;
    private Counter _counter;
    private HttpClient _client;

    [SetUp]
    public void SetUp()
    {
        CounterController.Counters.Clear();
        var factory = new WebApplicationFactory<Program>();
        _client = factory.CreateClient();
    }

    [Test]
    public async Task ShouldCreateACounter()
    {
        var response = await _client.PostAsync("/counters/foo", null);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

        var data = await response.Content.ReadFromJsonAsync<Counter>();
        Assert.That(data?.Name, Is.EqualTo("foo"));
        Assert.That(data?.Value,Is.EqualTo(0));
    }

    [Test]
    public async Task ShouldReturnConflictOnDuplicates()
    {
        var response1 = await _client.PostAsync("/counters/bar", null);
        Assert.That(response1.StatusCode,Is.EqualTo(HttpStatusCode.Created));

        var response2 = await _client.PostAsync("/counters/bar", null);
        Assert.That(response2.StatusCode,Is.EqualTo(HttpStatusCode.Conflict));
    }

    [Test]
    public async Task ShouldReadTheCounter()
    {
        var response = await _client.PostAsync("/counters/bin", null);
        Assert.That(response.StatusCode,Is.EqualTo(HttpStatusCode.Created));

        var response2 = await _client.GetAsync("/counters/bin");
        Assert.That(response2.StatusCode,Is.EqualTo(HttpStatusCode.OK));

        var data = await response2.Content.ReadFromJsonAsync<Counter>();
        Assert.That(data?.Name,Is.EqualTo("bin"));
        Assert.That(data?.Value,Is.EqualTo(0));
    }

    [Test]
    public async Task ShouldDeleteACounter()
    {
        var response = await _client.PostAsync("/counters/fob", null);
        Assert.That(response.StatusCode,Is.EqualTo(HttpStatusCode.Created));

        var response2 = await _client.DeleteAsync("/counters/fob");
        Assert.That(response2.StatusCode,Is.EqualTo(HttpStatusCode.NoContent));
    }
}