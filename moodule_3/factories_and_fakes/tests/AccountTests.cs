using factories_and_fakes.data;
using factories_and_fakes.models;
using factories_and_fakes.tests.factories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NUnit.Framework;

namespace factories_and_fakes.tests;

[TestFixture]
public class AccountsTests
{
    private AppDbContext _context;
    private Account account;

    private const string JsonPath =
        "/Users/katlegomaredi/Documents/code/edx_tdd_bdd_course/moodule_3/factories_and_fakes/tests/fixtures/accounts.json";

    private List<Account> accounts;

    [OneTimeSetUp]
    public void SetupOnce()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql("Host=localhost;Port=5432;Database=accountdb2;Username=postgres;Password=Kristine")
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

        var jsonData = File.ReadAllText(JsonPath);
        accounts = JsonConvert.DeserializeObject<List<Account>>(jsonData);
        account = new Account();
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
        accounts = JsonConvert.DeserializeObject<List<Account>>(jsonData);
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
        var account = AccountFactory.CreateAccount();
        account.Create(_context);

        Assert.That(Account.All(_context).Count, Is.EqualTo(1));
    }

    [Test]
    public void TestingCreatingAllAccounts()
    {
        for (int i = 0; i < 10; i++)
        {
            var account = AccountFactory.CreateAccount();
            account.Create(_context);
        }

        Assert.That(Account.All(_context).Count, Is.EqualTo(10));
    }

    [Test]
    public void TestingToStringMethod()
    {
        account.Name = "Foo";
        Assert.That(account.ToString(), Is.EqualTo("<Account Foo>"));
    }

    [Test]
    public void TestingToDictMethod()
    {
        var account = AccountFactory.CreateAccount();
        var result = account.ToDict();
        Assert.That(account.Name, Is.EqualTo(result["Name"]));
        Assert.That(account.Email, Is.EqualTo(result["Email"]));
        Assert.That(account.PhoneNumber, Is.EqualTo(result["PhoneNumber"]));
        Assert.That(account.Disabled, Is.EqualTo(result["Disabled"]));
        Assert.That(account.DateJoined, Is.EqualTo(result["DateJoined"]));
    }

    [Test]
    public void TestFromDictMethod()
    {
        var random = new Random();
        var data = accounts[random.Next(0, accounts.Count)];
        account = data;
        var result = account.ToDict();
        account.FromDict(result);
        Assert.That(result["Id"], Is.EqualTo(account.Id));
        Assert.That(result["Name"], Is.EqualTo(account.Name));
        Assert.That(result["Email"], Is.EqualTo(account.Email));
        Assert.That(result["PhoneNumber"], Is.EqualTo(account.PhoneNumber));
        Assert.That(result["Disabled"], Is.EqualTo(account.Disabled));
        Assert.That(result["DateJoined"], Is.EqualTo(account.DateJoined));
    }

    [Test]
    public void TestingUpdatingAccount()
    {
        var random = new Random();
        var data = accounts[random.Next(0, accounts.Count)];
        data.Create(_context);
        Assert.That(data.Id, Is.Not.Null);
        data.Name = "Foo";
        data.Update(_context);
        var found = Account.Find(_context, data.Id);
        Assert.That(found.Name, Is.EqualTo("Foo"));
    }

    [Test]
    public void TestingDataValidationError()
    {
        var random = new Random();
        var data = accounts[random.Next(0, accounts.Count)];
        data.Create(_context);
        data.Id = 0;
        var ex = Assert.Throws<DataValidationException>(() => data.Update(_context));
        Assert.That(ex.Message, Is.EqualTo("Update called with empty ID field"));
    }

    [Test]
    public void TestingDeletingAnAccount()
    {
        var random = new Random();
        var data = accounts[random.Next(0, accounts.Count)];
        data.Create(_context);
        Assert.That(Account.All(_context).Count, Is.EqualTo(1));
        data.Delete(_context);
        Assert.That(Account.All(_context).Count, Is.EqualTo(0));
    }
}