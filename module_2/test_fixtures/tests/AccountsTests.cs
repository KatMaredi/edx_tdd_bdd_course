using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NUnit.Framework;
using test_fixtures.data;
using test_fixtures.models;

namespace test_fixtures.tests;

[TestFixture]
public class AccountsTests
{
    private AppDbContext _context;
    private Account account;
    private const string JsonPath = "/Users/katlegomaredi/Documents/code/edx_tdd_bdd_course/module_2/test_fixtures/tests/fixtures/accounts.json";

    [OneTimeSetUp]
    public void SetupOnce()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql("Host=localhost;Port=5432;Database=accounts_db;Username=postgres;Password=Kristine")
            .Options;

        _context = new AppDbContext(options);

        _context.Database.EnsureCreated(); // Create the schema if it doesn't exist
        SeedDataFromJson(JsonPath);
    }

    [OneTimeTearDown]
    public void TeardownOnce()
    {
        _context.Dispose();
    }

    [SetUp]
    public void Setup()
    {
        _context.Accounts.RemoveRange(_context.Accounts);
        _context.SaveChanges();
    }

    [TearDown]
    public void Teardown()
    {
        _context.ChangeTracker.Clear();
    }

    private void SeedDataFromJson(string filePath)
    {
        if (!File.Exists(filePath)) return;

        var jsonData = File.ReadAllText(filePath);
        var accounts = JsonConvert.DeserializeObject<List<Account>>(jsonData);

        if (accounts != null && accounts.Any())
        {
            _context.Accounts.AddRange(accounts);
            _context.SaveChanges();
        }
    }
    
    //*********************************************************************************
    // TEST CASES
    //*********************************************************************************

        [Test]
        public void TestCreatingASingleAccount()
        {
            var jsonData = File.ReadAllText(JsonPath);
            var accounts = JsonConvert.DeserializeObject<List<Account>>(jsonData);

            var user = accounts[0];
            user.Create(_context);
            
            Assert.That(Account.All(_context).Count,Is.EqualTo(1));
        }
}