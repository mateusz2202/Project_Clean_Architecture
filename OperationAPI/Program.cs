using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using OperationAPI;
using OperationAPI.Data;
using OperationAPI.Interfaces;
using OperationAPI.Middleware;
using OperationAPI.Models;
using OperationAPI.Models.Validators;
using OperationAPI.Services;
using StackExchange.Redis;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("OperationAPIOperationAPI_Test")]

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(options =>
      options.SerializerSettings.ReferenceLoopHandling =
        Newtonsoft.Json.ReferenceLoopHandling.Ignore);

// Add services to the container.
builder.Services.AddFluentValidationAutoValidation();

//sql server
builder.Services.AddDbContext<OperationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnectionString")));

//cosmos db
builder.Services.AddSingleton<CosmosClient>((s) => new CosmosClient(builder.Configuration.GetConnectionString("CosmosDbConnectionString")));

//redis
builder.Services.AddSingleton<IDatabase>(cfg =>
{
    var redisConnection =
        ConnectionMultiplexer
            .Connect(builder.Configuration.GetConnectionString("RedisConnectionString") ?? string.Empty);

    return redisConnection.GetDatabase();
});

//rabbitMQ
builder.Services.Configure<RabbitMqConfiguration>(conf => builder.Configuration.GetSection(nameof(RabbitMqConfiguration)).Bind(conf));
builder.Services.AddSingleton<IRabbitMqService, RabbitMqService>();

//services
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<IOperationService, OperationService>();

////validator
builder.Services.AddScoped<IValidator<CreateOperationDTO>, CreateOperationDTOValidator>();
builder.Services.AddScoped<IValidator<CreateOperationWithAttributeDTO>, CreateOperationWithAttributeDTOValidator>();

//midleware
builder.Services.AddScoped<ErrorHandlingMiddleware>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ErrorHandlingMiddleware>();

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

