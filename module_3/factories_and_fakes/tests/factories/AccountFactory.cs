using Bogus;
using factories_and_fakes.models;

namespace factories_and_fakes.tests.factories;

public class AccountFactory
{
    private static Faker<Account> faker = new Faker<Account>()
        .RuleFor(a => a.Id, f => 0)
        .RuleFor(a => a.Name, f => f.Name.FullName())
        .RuleFor(a => a.Email, f => f.Internet.Email())
        .RuleFor(a => a.PhoneNumber, f => f.Phone.PhoneNumber("##########"))
        .RuleFor(a => a.Disabled, f => f.Random.Bool(0.1f))
        .RuleFor(a => a.DateJoined, f => f.Date.Past(3, DateTime.UtcNow));

    public static Account CreateAccount()
    {
        return faker.Generate();
    }

    // public static List<Account> CreateMany(int amount)
    // {
    //     return faker.Generate(amount);
    // }
}