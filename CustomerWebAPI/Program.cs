using Customer.BusinessLogic;
using Customer.BusinessLogic.Authentication;
using Customer.BusinessLogic.JsonSerializers;
using Customer.BusinessLogic.Logging;
using Customer.BusinessLogic.Mapper;
using Customer.BusinessLogic.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<CustomerDb>(options => options.UseInMemoryDatabase("CustomersWebApi"));
builder.Services.AddSingleton<IApiKeyValidation, ApiKeyValidation>();
builder.Services.AddMappings();
// Add each and every validator one after another
//builder.Services.AddScoped<IValidator<UpsertCustomerDto>, UpsertCustomerValidator>();
//Register all the validators in one go
builder.Services.AddValidatorsFromAssemblyContaining<UpsertCustomerValidator>();

builder.Services.AddControllers()
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.PropertyNamingPolicy = new SnakeCaseNamingPolicy();
    });

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

//Simple API key authentication middleware
//app.UseMiddleware<ApiKeyMiddleware>();
//app.UseMiddleware<RequestLoggingMiddleWare>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
