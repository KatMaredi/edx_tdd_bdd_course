using Microsoft.EntityFrameworkCore;
using test_fixtures.data;

var builder=WebApplication.CreateBuilder(args);

//Add services
builder.Services.AddDbContext<AppDbContext>(options=>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.Run();    