using aka;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

// app.UseAuthorization();

app.MapControllers();

var dataFolder = Environment.GetEnvironmentVariable("APP_DATA") ?? Environment.CurrentDirectory;

var dataUrl = Path.Combine(dataFolder, "data.txt");
Console.WriteLine($"Data file: {dataUrl}");

DataController.shared = new DataController(dataUrl);

app.Run();
