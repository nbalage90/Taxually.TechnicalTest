using Microsoft.Extensions.Configuration;
using Taxually.TechnicalTest;
using Taxually.TechnicalTest.Factories;
using Taxually.TechnicalTest.Handlers.ExceptionHandlers;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<ITaxuallyHttpClient, TaxuallyHttpClient>();
builder.Services.AddTransient<ITaxuallyQueueClient, TaxuallyQueueClient>();
builder.Services.AddTransient<CountryFactory>();
builder.Services.AddSingleton<IConfiguration>(provider => configuration);

builder.Services.AddExceptionHandler<VatRegistrationExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(_ => { });

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
