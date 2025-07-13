using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using NUnit.Framework;
using tdd_productRestApi.controllers;
using tdd_productRestApi.data;
using tdd_productRestApi.models;
using tdd_productRestApi.tests.factories;

namespace tdd_productRestApi.tests;

[TestFixture]
public class ProductControllerTests
{
    private AppDbContext _context;
    private Product _product;
    private HttpClient _client;
    
    [SetUp]
    public void Setup()
    {
        ProductsController.Products.Clear();
        var factory = new WebApplicationFactory<Program>();
        _client = factory.CreateClient();
    }

    [Test]
    public async Task ShouldReadAProduct()
    {
        var newProduct = ProductFactory.CreateProduct();
        var json = JsonConvert.SerializeObject(newProduct);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("http://localhost:5044/api/Products", content);
        Assert.That(response.StatusCode,Is.EqualTo(HttpStatusCode.Created));

        var result = await response.Content.ReadFromJsonAsync<Product>();
        var id = result.Id;
        var response2 = await _client.GetAsync($"http://localhost:5044/api/Products/{id}");
        Assert.That(response2.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var data = await response2.Content.ReadFromJsonAsync<Product>();
        Assert.That(data?.Name, Is.EqualTo(newProduct.Name));
    }
}