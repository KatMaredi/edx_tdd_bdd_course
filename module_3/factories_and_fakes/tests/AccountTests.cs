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

    [OneTimeSetUp]
    public void SetupOnce()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql("Host=localhost;Port=5432;Database=accountdb2;Username=postgres;Password=Kristine")
            .Options;

        _context = new AppDbContext(options);

        _context.Database.EnsureCreated(); // Create the schema if it doesn't exist
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
        account = new Account();
    }

    [TearDown]
    public void Teardown()
    {
        _context.ChangeTracker.Clear();
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
        var data = AccountFactory.CreateAccount().ToDict();
        account.FromDict(data);
        Assert.That(data["Id"], Is.EqualTo(account.Id));
        Assert.That(data["Name"], Is.EqualTo(account.Name));
        Assert.That(data["Email"], Is.EqualTo(account.Email));
        Assert.That(data["PhoneNumber"], Is.EqualTo(account.PhoneNumber));
        Assert.That(data["Disabled"], Is.EqualTo(account.Disabled));
        Assert.That(data["DateJoined"], Is.EqualTo(account.DateJoined));
    }

    [Test]
    public void TestingUpdatingAccount()
    {
        var data = AccountFactory.CreateAccount();
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
        var data = AccountFactory.CreateAccount();
        data.Create(_context);
        data.Id = 0;
        var ex = Assert.Throws<DataValidationException>(() => data.Update(_context));
        Assert.That(ex.Message, Is.EqualTo("Update called with empty ID field"));
    }

    [Test]
    public void TestingDeletingAnAccount()
    {
        var data = AccountFactory.CreateAccount();
        data.Create(_context);
        Assert.That(Account.All(_context).Count, Is.EqualTo(1));
        data.Delete(_context);
        Assert.That(Account.All(_context).Count, Is.EqualTo(0));
    }
}