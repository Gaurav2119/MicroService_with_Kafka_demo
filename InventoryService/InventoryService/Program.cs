using FluentValidation;
using InventoryService.AppDbContext;
using InventoryService.DataAccess.Implementation;
using InventoryService.DataAccess.Interface;
using InventoryService.KafkaProducer;
using InventoryService.Validations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//configure db
builder.Services.AddDbContext<productDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.Configure<KafkaProducerConfig>(builder.Configuration.GetSection("KafkaProducer"));

builder.Services.AddScoped<IInventory, Inventory>();
builder.Services.AddSingleton<IProducer, Producer>();
builder.Services.AddScoped<IValidator<InventoryService.Models.Inventory>, InventoryValidations>();

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

app.Run();
