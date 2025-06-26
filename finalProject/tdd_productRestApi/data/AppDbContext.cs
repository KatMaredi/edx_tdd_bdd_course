using Microsoft.EntityFrameworkCore;
using tdd_productRestApi.models;

namespace tdd_productRestApi.data;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options){}
    
    public DbSet<Product> Products { get; set; }
}