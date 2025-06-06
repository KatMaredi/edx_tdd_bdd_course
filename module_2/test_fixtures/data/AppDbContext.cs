using Microsoft.EntityFrameworkCore;
using test_fixtures.models;

namespace test_fixtures.data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }
}