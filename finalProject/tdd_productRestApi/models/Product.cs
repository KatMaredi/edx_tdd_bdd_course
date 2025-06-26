using System.ComponentModel.DataAnnotations;
using tdd_productRestApi.data;

namespace tdd_productRestApi.models;

public class DataValidationException : Exception
{
    public DataValidationException(string message) : base(message)
    {
    }
}

public enum Category
{
    UNKNOWN,
    CLOTHS,
    FOOD,
    HOUSEWARES,
    AUTOMOTIVE,
    TOOLS
}

public class Product
{
    private static readonly ILogger<Product> _logger =
        LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<Product>();

    [Key] public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public bool Available { get; set; }
    public Category Category { get; set; }

    public override string ToString() => $"<Product name: {Name}, id: {Id}";

    public async Task CreateAsync(AppDbContext context)
    {
        _logger.LogInformation("Create product {Name}", Name);
        context.Products.Add(this);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(AppDbContext context)
    {
        _logger.LogInformation("Update product to the Db {Name}", Name);
        if (Id == 0)
        {
            throw new DataValidationException("Update called with empty Id field");
        }

        context.Products.Update(this);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(AppDbContext context)
    {
        _logger.LogInformation("Remove product from the store");
        context.Products.Remove(this);
       await context.SaveChangesAsync();
    }

    public Dictionary<string, object> ToDict()
    {
        return new Dictionary<string, object>
        {
            { "Id", Id },
            { "Name", Name },
            { "Description", Description },
            { "Price", Price },
            { "Available", Available },
            { "Category", Category }
        };
    }

    public void FromDict(Dictionary<string, object> data)
    {
        foreach (var kv in data)
        {
            switch (kv.Key)
            {
                case "Name":
                    Name = kv.Value as string;
                    break;
                case "Description":
                    Description = kv.Value as string;
                    break;
                case "Price":
                    Price = Convert.ToDouble(kv.Value);
                    break;
                case "Category":
                    Category = (Category)kv.Value;
                    break;
            }
        }
    }

    public static List<Product> FindAll(AppDbContext context)
    {
        _logger.LogInformation("Returns all of the products in the database");
        return context.Products.ToList();
    }

    public static Product FindById(AppDbContext context, int productId)
    {
        _logger.LogInformation("Finds a product by its Id");
        return context.Products.Find(productId);
    }

    public static List<Product> FindByName(AppDbContext context, string name)
    {
        _logger.LogInformation("Finds all products with a given Name");
        return context.Products.Where(p => p.Name == name)
            .ToList();
    }

    public static List<Product> FindByPrice(AppDbContext context, double price)
    {
        _logger.LogInformation("Finds all products with the given price");
        return context.Products.Where(p => p.Price == price)
            .ToList();
    }

    public static List<Product> FindByAvailability(AppDbContext context, bool availability)
    {
        _logger.LogInformation("Returns all products by their availability");
        return context.Products.Where(p => p.Available == availability)
            .ToList();
    }

    public static List<Product> FindByCategory(AppDbContext context, Category category)
    {
        _logger.LogInformation("Returns all products by category");
        return context.Products.Where(p => p.Category == category)
            .ToList();
    }
}