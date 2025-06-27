var builder = WebApplication.CreateBuilder(args);
builder.Services.AddConnections();
var app = builder.Build();
app.MapControllers();
app.Run();