using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using urlShortener.Utils;

MongoClientSettings settings = MongoClientSettings.FromConnectionString(
    Config.ATLAS_URI
);

settings.LinqProvider = LinqProvider.V3;

MongoClient client = new MongoClient(settings);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // Add this line to register controllers
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();

app.Run();