using implementingSteps.features.contexts;

namespace implementingSteps.features.steps;

using System.Net;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using SpecFlow.Internal.Json;
using TechTalk.SpecFlow;

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

    [Given("the following pets")]
    public async Task RefreshAllThePetsInTheDataBaseAsync(Table table)
    {
        // List all pets and delete them on by one
        var response = await _client.GetAsync($"{_context.BaseUrl}/pets");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var pets = JArray.Parse(await response.Content.ReadAsStringAsync());

        foreach (var pet in pets)
        {
            var petId = pet["id"]?.ToString();
            var deleteResponse = await _client.DeleteAsync($"{_context.BaseUrl}/pets/{petId}");
            Assert.That(deleteResponse.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }

        // Load the database with Pets
        foreach (var row in table.Rows)
        {
            var payload = new
            {
                name = row["name"],
                category = row["category"],
                available = row["available"].ToLower() switch
                {
                    "true" or "1" => true,
                    _ => false
                },
                gender = row["gender"],
                birthday = row["birthday"]
            };

            var postResponse = await _client.PostAsJsonAsync($"{_context.BaseUrl}/pets", payload);
            Assert.That(postResponse.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }
    }
}