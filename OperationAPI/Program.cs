using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using OperationAPI.Data;
using OperationAPI.Interfaces;
using OperationAPI.Middleware;
using OperationAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<OperationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnectionString")));
builder.Services.AddSingleton((s) => new CosmosClient(builder.Configuration.GetConnectionString("CosmosDbConnectionString")));

builder.Services.AddScoped<IOperationService, OperationService>();
builder.Services.AddScoped<IOperationAttributeService, OperationAttributeService>();

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
