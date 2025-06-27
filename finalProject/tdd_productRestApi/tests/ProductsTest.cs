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
    }
}