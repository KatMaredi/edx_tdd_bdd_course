using System.ComponentModel;
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

    [DisplayName(displayName:"Test if it reads a product")]
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

    [DisplayName(displayName: "Test if it updates product in the DB")]
    [Test]
    public async Task shouldUpdateProduct()
    {
        var newProduct = ProductFactory.CreateProduct();
        var json = JsonConvert.SerializeObject(newProduct);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("http://localhost:5044/api/Products", content);
        Assert.That(response.StatusCode,Is.EqualTo(HttpStatusCode.Created));

        var result = await response.Content.ReadFromJsonAsync<Product>();
        var id = result.Id;
        var response2 = await _client.GetAsync($"http://localhost:5044/api/Products/{id}");
        var productFromDB = await response2.Content.ReadFromJsonAsync<Product>();
        productFromDB.Description = "Test2";

        var json2 = JsonConvert.SerializeObject(productFromDB);
        var content2 = new StringContent(json2, Encoding.UTF8, "application/json");
        var response3 = await _client.PutAsync($"http://localhost:5044/api/Products/{id}", content2);
        Assert.That(response3.StatusCode,Is.EqualTo(HttpStatusCode.OK));
        
        var response4 = await _client.GetAsync($"http://localhost:5044/api/Products/{id}");
        var updatedProductFromDB = await response4.Content.ReadFromJsonAsync<Product>();
        Assert.That(updatedProductFromDB.Description,Is.EqualTo("Test2"));
    }

    [DisplayName(displayName: "Test if it deletes from the DB")]
    [Test]
    public async Task shouldDeleteProduct()
    {
        var newProduct = ProductFactory.CreateProduct();
        var json = JsonConvert.SerializeObject(newProduct);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync("http://localhost:5044/api/Products", content);
        Assert.That(response.StatusCode,Is.EqualTo(HttpStatusCode.Created));

        var result = await response.Content.ReadFromJsonAsync<Product>();
        var id = result.Id;

        var response2 = await _client.DeleteAsync($"http://localhost:5044/api/Products/{id}");
        Assert.That(response2.StatusCode,Is.EqualTo(HttpStatusCode.OK));
    }
}