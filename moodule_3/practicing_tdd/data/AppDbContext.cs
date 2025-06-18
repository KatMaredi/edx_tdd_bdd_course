using Microsoft.EntityFrameworkCore;
using practicing_tdd.models;

namespace practicing_tdd.data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Counter> Counters { get; set; }
}