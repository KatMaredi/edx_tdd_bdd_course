using Microsoft.EntityFrameworkCore;
using test_coverage.models;

namespace test_coverage.data;

public class AppDbContext :DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }
}