using Bogus;
using tdd_productRestApi.models;

namespace tdd_productRestApi.tests.factories;

public class ProductFactory
{
    private static Faker<Product> faker = new Faker<Product>()
        .RuleFor(p => p.Id, f => 0)
        .RuleFor(p => p.Name, f => f.Commerce.ProductName())
        .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
        .RuleFor(p => p.Price, f => Convert.ToDouble(f.Commerce.Price()))
        .RuleFor(p => p.Available, f => f.Random.Bool(0.4f))
        .RuleFor(p => p.Category, f => f.Random.Enum(Category.UNKNOWN));

    public static Product CreateProduct()
    {
        return faker.Generate();
    }

    public static List<Product> CreateBatch(int num)
    {
        return faker.Generate(num);
    }
}