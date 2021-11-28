using aka;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var dataFolder = Environment.GetEnvironmentVariable("APP_DATA") ?? Environment.CurrentDirectory;
var dataUrl = Path.Combine(dataFolder, "data.txt");
// var dataController = new DataController(dataUrl);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<DataController>(x => new DataController(x.GetRequiredService<ILogger<DataController>>(), dataUrl));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

// app.UseAuthorization();

app.UseDefaultFiles();

app.UseStaticFiles();

// use controller when url doesn't start with /ui
app.MapWhen(context => !context.Request.Path.ToUriComponent().StartsWith("/ui"), appBranch =>
{
    appBranch.UseRouting();
    appBranch.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
    });
});

app.Run();
