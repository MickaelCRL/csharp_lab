using ComputerManagement.API;
using ComputerManagement.API.Events;
using ComputerManagement.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FleetContext>();

builder.Services.AddScoped<IComputerService, ComputerService>();
builder.Services.AddScoped<IRepairService, RepairService>();

builder.Services.AddSingleton<StockService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var computerService = scope.ServiceProvider.GetRequiredService<IComputerService>();

    // Subscriptions
    computerService.ComputerAdded += DisplayAddMessage;
    computerService.ComputerUpdated += DisplayUpdateMessage;
    computerService.ComputerDeleted += DisplayDeleteMessage;
}

// Subscriber methods
void DisplayAddMessage(object? sender, ComputerAddedEventArgs e)
{
    Console.WriteLine($"[EVENT] The computer '{e.Computer.Name}' ({e.Computer.Brand}) was successfully added.");
}

void DisplayUpdateMessage(object? sender, ComputerUpdatedEventArgs e)
{
    Console.WriteLine($"[EVENT] The computer with ID {e.Computer.Id} was updated.");
}

void DisplayDeleteMessage(object? sender, ComputerDeletedEventArgs e)
{
    Console.WriteLine($"[EVENT] The computer with ID {e.ComputerId} was deleted from the fleet.");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();