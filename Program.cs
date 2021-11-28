using aka;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var dataFolder = Environment.GetEnvironmentVariable("APP_DATA") ?? Environment.CurrentDirectory;
var dataUrl = Path.Combine(dataFolder, "data.txt");
var dataController = new DataController(dataUrl);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<DataController>(dataController);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();

app.UseStaticFiles();

// app.UseHttpsRedirection();

// app.UseAuthorization();

app.MapControllers();

app.Run();
