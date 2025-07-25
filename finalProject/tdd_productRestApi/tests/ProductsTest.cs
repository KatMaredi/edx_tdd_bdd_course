using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using tdd_productRestApi.data;
using tdd_productRestApi.models;
using tdd_productRestApi.tests.factories;

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

    [Test]
    public async Task ShouldAddAProductAndAddToTheDatabase()
    {
        var allProducts = Product.FindAll(_context);
        Assert.That(allProducts,Is.Empty);

        var createdProduct = ProductFactory.CreateProduct();
        await createdProduct.CreateAsync(_context);
        Assert.That(createdProduct.Id,Is.Not.Null);
        var productsFromDb = Product.FindAll(_context);
        Assert.That(productsFromDb.Count,Is.EqualTo(1));
       
        Assert.That(productsFromDb[0].Name,Is.EqualTo(createdProduct.Name));
        Assert.That(productsFromDb[0].Description,Is.EqualTo(createdProduct.Description));
        Assert.That(productsFromDb[0].Price,Is.EqualTo(createdProduct.Price));
        Assert.That(productsFromDb[0].Available,Is.EqualTo(createdProduct.Available));
        Assert.That(productsFromDb[0].Category,Is.EqualTo(createdProduct.Category));
    }

    [Test]
    public async Task ShouldRetrieveProductFromTheDatabase()
    {
        var createdProduct = ProductFactory.CreateProduct();
        await createdProduct.CreateAsync(_context);
        Assert.That(createdProduct.Id,Is.Not.Null);

        var productFromDb = await Product.FindByIdAsync(_context, createdProduct.Id);
        
        Assert.That(productFromDb.Name,Is.EqualTo(createdProduct.Name));
        Assert.That(productFromDb.Description,Is.EqualTo(createdProduct.Description));
        Assert.That(productFromDb.Price,Is.EqualTo(createdProduct.Price));
        Assert.That(productFromDb.Available,Is.EqualTo(createdProduct.Available));
        Assert.That(productFromDb.Category,Is.EqualTo(createdProduct.Category));
    }

    [Test]
    public async Task ShouldUpdateProductInTheDb()
    {
        var createdProduct = ProductFactory.CreateProduct();
        await createdProduct.CreateAsync(_context);
        
        Assert.That(createdProduct.Id,Is.Not.Null);

        createdProduct.Description = "Testing";
        var originalId = createdProduct.Id;
        await createdProduct.UpdateAsync(_context);
        
        Assert.That(createdProduct.Id,Is.EqualTo(originalId));
        Assert.That(createdProduct.Description, Is.EqualTo("Testing"));

        var allProductsFromDb = Product.FindAll(_context);
        
        Assert.That(allProductsFromDb.Count,Is.EqualTo(1));
        Assert.That(allProductsFromDb[0].Id,Is.EqualTo(originalId));
        Assert.That(allProductsFromDb[0].Description,Is.EqualTo("Testing"));
    }

    [Test]
    public async Task ShouldDeleteProductFromTheDb()
    {
        var createdProduct = ProductFactory.CreateProduct();
        await createdProduct.CreateAsync(_context);
        Assert.That(Product.FindAll(_context).Count,Is.EqualTo(1));

        await createdProduct.DeleteAsync(_context);
        
        Assert.That(Product.FindAll(_context),Is.Empty);
    }

    [Test]
    public async Task ShouldListAllProductsFromTheDb()
    {
        Assert.That(Product.FindAll(_context),Is.Empty);

        for (int i = 0; i < 5; i++)
        {
            var createdProduct = ProductFactory.CreateProduct();
            await createdProduct.CreateAsync(_context);
        }
        
        Assert.That(Product.FindAll(_context).Count,Is.EqualTo(5));
    }

    [Test]
    public async Task ShouldFindProductByName()
    {
        var createdProducts = ProductFactory.CreateBatch(5);

        foreach (var product in createdProducts)
        {
            await product.CreateAsync(_context);
        }

        createdProducts[0].Name = "Test";
        createdProducts[1].Name = "Test";
        await createdProducts[0].UpdateAsync(_context);
        await createdProducts[1].UpdateAsync(_context);
        
        var productsFromDb=Product.FindByName(_context,"Test");
        Assert.That(productsFromDb.Count,Is.EqualTo(2));
    }

    [Test]
    public async Task ShouldFindProductByAvailability()
    {
        var createdProducts = ProductFactory.CreateBatch(10);
        foreach (var product in createdProducts)
        {
           await product.CreateAsync(_context);
        }

        var productsFromDb = Product.FindByAvailability(_context, true);
        foreach (var productFromDb in productsFromDb)
        {
            Assert.That(productFromDb.Available,Is.True);
        }
    }
    
    [Test]
    public async Task ShouldFindProductByCategory()
    {
        var createdProducts = ProductFactory.CreateBatch(10);
        foreach (var product in createdProducts)
        {
            await product.CreateAsync(_context);
        }

        var productsFromDb = Product.FindByCategory(_context, Category.AUTOMOTIVE);
        foreach (var productFromDb in productsFromDb)
        {
            Assert.That(productFromDb.Category,Is.EqualTo(Category.AUTOMOTIVE));
        }
    }
}