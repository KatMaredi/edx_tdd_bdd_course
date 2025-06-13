using System.Net;
using mocking_objects.models;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace mocking_objects.test;

[TestFixture]
public class ImdbTests
{
    private static Dictionary<string, JObject> imdbData;
    private Mock<HttpMessageHandler> _handlerMock;
    private HttpClient _httpClient;
    private Imdb _imdb;
    private string fakeResponse;
    private HttpStatusCode httpCode = HttpStatusCode.OK;

    [OneTimeSetUp]
    public void LoadFixtures()
    {
        var json = File.ReadAllText(
            "/Users/katlegomaredi/Documents/code/edx_tdd_bdd_course/moodule_3/mocking_objects/test/fixtures/imdb_responses.json");
        imdbData = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(json);
    }

    [SetUp]
    public void SetUp()
    {
        httpCode = HttpStatusCode.OK;
        fakeResponse = imdbData["GOOD_SEARCH"].ToString();
        BuildImdb();
    }

    private void BuildImdb()
    {
        _handlerMock = new Mock<HttpMessageHandler>();
        _handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = httpCode,
                Content = new StringContent(fakeResponse)
            });

        _httpClient = new HttpClient(_handlerMock.Object);
        var loggerMock = new Mock<ILogger<Imdb>>();
        _imdb = new Imdb(_httpClient, loggerMock.Object, "FAKE_API_KEY");
    }

    [Test]
    public async Task TestingGoodSearchWhenSearchingForTitle()
    {
        fakeResponse = imdbData["GOOD_SEARCH"].ToString();

        var result = await _imdb.SearchTitlesAsync("Bambi");

        Assert.That(result, Is.Not.Null);
        Assert.That(result["errorMessage"], Is.Null);

        var resultsList = result["results"] as JArray;
        Assert.That(resultsList, Is.Not.Null);
        Assert.That(resultsList[0]["id"].ToString(), Is.EqualTo("tt1375666"));
    }
    
}