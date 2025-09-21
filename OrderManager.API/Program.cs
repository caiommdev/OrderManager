using OrderManager.API.Application.Services.Interfaces;
using OrderManager.API.Application.Services.Interfaces.Factories;
using OrderManager.API.Application.Services.Interfaces.Validation;
using OrderManager.API.Application.Services;
using OrderManager.API.Application.Services.Validation;
using OrderManager.API.Application.Services.Factories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IValidationService, ValidationService>();
builder.Services.AddScoped<IDeliveryService, DeliveryService>();
builder.Services.AddScoped<ILabelService, LabelService>();
builder.Services.AddScoped<IShippingCalculatorFactory, ShippingCalculatorFactory>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
