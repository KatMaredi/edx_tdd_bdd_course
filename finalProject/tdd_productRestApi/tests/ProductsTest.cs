using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using tdd_productRestApi.data;
using tdd_productRestApi.models;

namespace tdd_productRestApi.tests;

[TestFixture]
public class ProductsTest
{
    private AppDbContext _context;
    private Product product;

    [OneTimeSetUp]
    public void SetUpOnce()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql("Host=localhost;Port=5432;Database=productDb;Username=postgres;Password=Kristine")
            .Options;

        _context = new AppDbContext(options);

        _context.Database.EnsureCreated();
    }

    [OneTimeTearDown]
    public void TeardownOnce()
    {
        _context.Dispose();
    }

    [SetUp]
    public void Setup()
    {
        _context.Products.RemoveRange(_context.Products);
        _context.SaveChanges();
        product = new Product();
    }

    [TearDown]
    public void Teardown()
    {
        _context.ChangeTracker.Clear();
    }

    /*******************************************************************************************************************
     * TEST CASES
     ******************************************************************************************************************/

    [Test]
    public void ShouldCreateAProduct()
    {
        product.Name = "Fedora";
        product.Description = "A red hat";
        product.Price = 12.50;
        product.Available = true;
        product.Category = Category.CLOTHS;
        
        Assert.That(product.ToString(),Is.EqualTo($"<Product name: Fedora, id: 0"));
        Assert.That(product,Is.Not.Null);
        Assert.That(product.Id,Is.EqualTo(0));
        Assert.That(product.Name,Is.EqualTo("Fedora"));
        Assert.That(product.Description,Is.EqualTo("A red hat"));
        Assert.That(product.Price,Is.EqualTo(12.50));
        Assert.That(product.Available,Is.EqualTo(true));
        Assert.That(product.Category,Is.EqualTo(Category.CLOTHS));
    }
}