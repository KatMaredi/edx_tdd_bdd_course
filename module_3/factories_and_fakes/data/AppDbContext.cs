using factories_and_fakes.models;
using Microsoft.EntityFrameworkCore;

namespace factories_and_fakes.data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }
}