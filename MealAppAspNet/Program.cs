// using MealAppAspNet.Models;
// using MealAppAspNet.Services;
using MealAppAspNet.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);

// Add services to our app.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure MongoDB settings
// builder.Services.Configure<MongoDbSettings>(
//     builder.Configuration.GetSection("MongoDbSettings"));
// builder.Services.AddSingleton<MealService>();

// Ef and Postgresql configuration
builder.Services.AddDbContext<MealContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


// Add CORS for React app
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.UseStaticFiles(); // handle files from wwwroot
app.UseCors("AllowReactApp");
app.UseAuthorization();
app.MapControllers();

app.Run();
