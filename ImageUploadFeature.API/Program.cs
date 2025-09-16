using ImageUploadFeature.API.Data;
using ImageUploadFeature.API.Entities;
using ImageUploadFeature.API.Interfaces;
using ImageUploadFeature.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("ImageUploadFeatureDb"));

builder.Services.AddScoped<IProfileService, ProfileService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ImageUploadFeature API", Version = "v1" });
});

var app = builder.Build();

// Seed default Customer & Lead
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!db.Customers.Any())
    {
        db.Customers.Add(new Customer
        {
            Id = Guid.Parse("40815d25-42da-4144-8a66-3f29fa3c726b"),
            Name = "Default Customer"
        });
    }
    if (!db.Leads.Any())
    {
        db.Leads.Add(new Lead
        {
            Id = Guid.Parse("97c4e6c5-b19d-47da-9779-ad0863d484d7"),
            Name = "Default Lead"
        });
    }

    db.SaveChanges();
}

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ImageUploadFeature API v1"));

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
