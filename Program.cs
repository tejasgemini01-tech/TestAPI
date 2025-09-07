using demo_api.Services;
using demo_api.Models;
using demo_api.Services;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://*:" + Environment.GetEnvironmentVariable("PORT"));
var MyReactAppPolicy = "MyReactAppPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyReactAppPolicy,
        policy =>
        {
            // For development, you can allow any origin.
            // For production, you should restrict it to your actual React app's domain.
            // policy.WithOrigins("http://localhost:3000")
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Add services to the container.
// Map the MongoDBSettings section in appsettings.json to the MongoDBSettings class.
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDBSettings"));
// Register MongoDbService as a singleton.
builder.Services.AddSingleton<MongoDbService>();

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

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors("MyReactAppPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
