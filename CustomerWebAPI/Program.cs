using Customer.BusinessLogic;
using Customer.BusinessLogic.Authentication;
using Customer.BusinessLogic.Mapper;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<CustomerDb>(options => options.UseInMemoryDatabase("CustomersWebApi"));
builder.Services.AddSingleton<IApiKeyValidation, ApiKeyValidation>();
builder.Services.AddMappings();
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

//Simple API key authentication middleware
//app.UseMiddleware<ApiKeyMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
