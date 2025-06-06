using test_fixtures.data;

namespace test_fixtures.models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

public class DataValidationException : Exception
{
    public DataValidationException(string message) : base(message) { }
}

public class Account
{
    private static readonly ILogger<Account> _logger =
        LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<Account>();

    [Key]
    public int Id { get; set; }

    [MaxLength(64)]
    public string Name { get; set; }

    [MaxLength(64)]
    public string Email { get; set; }

    [MaxLength(32)]
    public string PhoneNumber { get; set; }

    [Required]
    public bool Disabled { get; set; } = false;

    [Required]
    public DateTime DateJoined { get; set; } = DateTime.UtcNow;

    public override string ToString() => $"<Account {Name}>";

    public Dictionary<string, object> ToDict()
    {
        return new Dictionary<string, object>
        {
            { "Id", Id },
            { "Name", Name },
            { "Email", Email },
            { "PhoneNumber", PhoneNumber },
            { "Disabled", Disabled },
            { "DateJoined", DateJoined }
        };
    }

    public void FromDict(Dictionary<string, object> data)
    {
        foreach (var kv in data)
        {
            switch (kv.Key)
            {
                case "Name": Name = kv.Value as string; break;
                case "Email": Email = kv.Value as string; break;
                case "PhoneNumber": PhoneNumber = kv.Value as string; break;
                case "Disabled": Disabled = Convert.ToBoolean(kv.Value); break;
                case "DateJoined": DateJoined = Convert.ToDateTime(kv.Value); break;
                // Skip Id as it's usually set by the database
            }
        }
    }

    public void Create(AppDbContext context)
    {
        _logger.LogInformation("Creating {Name}", Name);
        context.Accounts.Add(this);
        context.SaveChanges();
    }

    public void Update(AppDbContext context)
    {
        _logger.LogInformation("Saving {Name}", Name);
        if (Id == 0)
        {
            throw new DataValidationException("Update called with empty ID field");
        }
        context.Accounts.Update(this);
        context.SaveChanges();
    }

    public void Delete(AppDbContext context)
    {
        _logger.LogInformation("Deleting {Name}", Name);
        context.Accounts.Remove(this);
        context.SaveChanges();
    }

    public static List<Account> All(AppDbContext context)
    {
        _logger.LogInformation("Processing all Accounts");
        return context.Accounts.ToList();
    }

    public static Account Find(AppDbContext context, int accountId)
    {
        _logger.LogInformation("Processing lookup for id {AccountId} ...", accountId);
        return context.Accounts.Find(accountId);
    }
}
