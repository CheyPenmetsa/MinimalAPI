using Customer.BusinessLogic;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Customer.BusinessLogic.Mapper;
using Customer.BusinessLogic.DTOs;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Customer.BusinessLogic.Authentication;
using Customer.BusinessLogic.Logging;
using Customer.BusinessLogic.Validators;
using FluentValidation;
using CustomerMinimalAPI;
using Customer.BusinessLogic.JsonSerializers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CustomerDb>(opt => opt.UseInMemoryDatabase("CustomersMinimal"));
builder.Services.AddSingleton<IApiKeyValidation, ApiKeyValidation>();
builder.Services.AddMappings();
// Add each and every validator one after another
//builder.Services.AddScoped<IValidator<UpsertCustomerDto>, UpsertCustomerValidator>();
//Register all the validators in one go
builder.Services.AddValidatorsFromAssemblyContaining<UpsertCustomerValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Customer Minimal API", Description = "Perform actions on Customer", Version = "v1" });
});

builder.Services.Configure<JsonOptions>(opt =>
{
    opt.JsonSerializerOptions.PropertyNamingPolicy = new SnakeCaseNamingPolicy();
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer Minimal API V1");
});

//Simple API key authentication middleware
//app.UseMiddleware<ApiKeyMiddleware>();
//app.UseMiddleware<RequestLoggingMiddleWare>();

app.Use(async (context, next) => 
{
    //Modify response by adding custom headers for cache invalidation on client side.
    await next();
});

app.MapGet("/customer", async (CustomerDb db, IMapper _mapper) =>
{
    var customerEntities = await db.Customers.ToListAsync();
    return _mapper.Map<List<CustomerDto>>(customerEntities);
}).AddEndpointFilter<ApiKeyEndpointFilter>();
    

app.MapGet("/customer/{id}", async ([FromRoute] int id, CustomerDb db, IMapper _mapper) =>
{
    return await db.Customers.FindAsync(id) is CustomerEntity customer
            ? Results.Ok(_mapper.Map<CustomerDto>(customer))
            : Results.NotFound();
}).WithMetadata(new EndpointNameMetadata("GetCustomerById"))
.AllowAnonymous();

app.MapPost("/customer", async ([FromBody]UpsertCustomerDto upsertCustomerDto, 
    CustomerDb db, 
    LinkGenerator links, 
    HttpContext httpContext,
    IValidator<UpsertCustomerDto> validator) =>
{
    FluentValidation.Results.ValidationResult result = validator.Validate(upsertCustomerDto);

    if (!result.IsValid)
    {
        return Results.ValidationProblem(result.ToDictionary());
    }

    var currentCount = await db.Customers.CountAsync();
    var customerEntity = upsertCustomerDto.Adapt<CustomerEntity>();
    customerEntity.Id = currentCount + 1;
    db.Customers.Add(customerEntity);
    await db.SaveChangesAsync();
    //return Results.CreatedAtRoute("GetCustomerById", new { id = customerEntity.Id }, customerEntity);
    return Results.Created(links.GetUriByName(httpContext, "GetCustomerById", new { id = customerEntity.Id })!, customerEntity);
}).RequireAuthorization("ApiKeyPolicy");

app.MapPut("/customer/{id}", async ([FromRoute] int id, [FromBody] UpsertCustomerDto upsertCustomerDto, CustomerDb db) =>
{
    var customerEntity = await db.Customers.FindAsync(id);

    if (customerEntity is null) return Results.NotFound();

    customerEntity.Adapt(upsertCustomerDto);
    await db.SaveChangesAsync();
    return Results.NoContent();
}).AddEndpointFilter<ValidationFilter<UpsertCustomerDto>>(); ;

app.MapDelete("/customer/{id}", async ([FromRoute] int id, CustomerDb db) =>
{
    if (await db.Customers.FindAsync(id) is CustomerEntity customer)
    {
        db.Customers.Remove(customer);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }

    return Results.NotFound();
});

app.Run();
