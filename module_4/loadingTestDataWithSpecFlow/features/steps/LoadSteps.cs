using System.Net;
using loadingTestDataWithSpecFlow.features.contexts;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using SpecFlow.Internal.Json;
using TechTalk.SpecFlow;

namespace loadingTestDataWithSpecFlow.features.steps;

[Binding]
public class LoadSteps
{
    private readonly Context _context;
    private HttpClient _client;
    
    public LoadSteps(Context context)
    {
        _context = context;
        _client = new HttpClient();
    }

    [Given("Given the following pets")]
    public async Task RefreshAllThePetsInTheDataBaseAsync()
    {
        // List all pets and delete them on by one
        var response = await _client.GetAsync($"{_context.BaseUrl}/pets");
        Assert.That(response.StatusCode,Is.EqualTo(HttpStatusCode.OK));

        var pets = JArray.Parse(await response.Content.ReadAsStringAsync());

        foreach (var pet in pets)
        {
            var petId = pet["id"]?.ToString();
            var deleteResponse = await _client.DeleteAsync($"{_context.BaseUrl}/pets/{petId}");
            Assert.That(deleteResponse.StatusCode,Is.EqualTo(HttpStatusCode.NoContent));
        }
    }
}