using API.Extensions;
using API.Middleware;
using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplicationService(builder);
builder.Services.AddSwaggerGen();
builder.Services.AddDbContextFactory<DataContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(builder.Configuration["Local"]) ?? 
        throw new InvalidOperationException("Connection string not found.")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    app.UseMiddleware<ExceptionMiddleware>(context);
}
catch (Exception e)
{
    var logger = services.GetService<ILogger<Program>>();
    logger!.LogError(e, "An error occurred during db migration");
    Console.WriteLine(e);
    throw;
}

app.Run();