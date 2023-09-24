using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OperationAPI.Data;
using OperationAPI.Entities;
using OperationAPI.Models;
using OperationAPIOperationAPI_Test;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace OperationAPI.Controllers.Tests;

public class OperationControllerTests
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public OperationControllerTests()
    {
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices((conf,services) =>
                {
                    //sql server change to memoryDatabase
                    var dbContextOption = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<OperationDbContext>));

                    if (dbContextOption != null)
                        services.Remove(dbContextOption);

                    services.AddDbContext<OperationDbContext>(options => options.UseInMemoryDatabase("OperationDb"));                  

                });
            });

        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task GetAllOperationsWithAtrributes()
    {
        var createdOperation = await CreateOperation();

        var attribute = new
        {
            id = createdOperation.Id.ToString(),
            code = createdOperation.Code,
            height = "something",
            width = "something",
            weight = "something",
        };

        var responseCreatedAttribute = await _client.PostAsJsonAsync("/Operation/attributes", attribute);
        responseCreatedAttribute.EnsureSuccessStatusCode();

        var response = await _client.GetAsync("/Operation");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var operations = await response.Content.ReadFromJsonAsync<IEnumerable<object>>();

        operations.Should().NotBeNull();
        operations.Should().HaveCountGreaterThan(0);

        await Task.CompletedTask;
    }

    [Fact]
    public async Task GetAllOperationsTest()
    {
        var tasks = Enumerable.Range(0, 10).Select(_ => CreateOperation());
        await Task.WhenAll(tasks);

        var response = await _client.GetAsync("/Operation/operations");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var operations = await response.Content.ReadFromJsonAsync<IEnumerable<Operation>>();

        operations.Should().NotBeNull();
        operations.Should().HaveCountGreaterThan(9);

        await Task.CompletedTask;
    }

    [Fact]
    public async Task GetAllAttributesTest()
    {
        var createdOperation = await CreateOperation();

        var attribute = new
        {
            id = createdOperation.Id.ToString(),
            code = createdOperation.Code,
            height = "something",
            width = "something",
            weight = "something",
        };

        var responseCreatedAttribute = await _client.PostAsJsonAsync("/Operation/attributes", attribute);
        responseCreatedAttribute.EnsureSuccessStatusCode();

        var response = await _client.GetAsync("/Operation/attributes");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var operations = await response.Content.ReadFromJsonAsync<IEnumerable<object>>();

        operations.Should().NotBeNull();
        operations.Should().HaveCountGreaterThan(0);

        await Task.CompletedTask;
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public async Task GetOperationByIdTest_NotFound(int id)
    {
        var response = await _client.GetAsync($"/Operation/operations/id/{id}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        await Task.CompletedTask;
    }

    [Fact]
    public async Task GetOperationByIdTest_Ok()
    {
        var createdOperation = await CreateOperation();

        var response = await _client.GetAsync($"/Operation/operations/id/{createdOperation.Id}");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var operation = await response.Content.ReadFromJsonAsync<Operation>();

        operation.Should().NotBeNull();
        operation.Should().BeEquivalentTo(createdOperation);

        await Task.CompletedTask;
    }

    [Fact]
    public async Task DeleteOperationByIdTest()
    {
        var createdOperation = await CreateOperation();

        var responseDelete = await _client.DeleteAsync($"/Operation/id/{createdOperation.Id}");

        responseDelete.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

        var response = await _client.GetAsync($"/Operation/operations/id/{createdOperation.Id}");

        response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);

    }

    private async Task<Operation> CreateOperation()
    {
        var ranodmCode = RandomGenerator.RandomString(4);

        var createdOperation = new CreateOperationDTO()
        {
            Code = ranodmCode,
            Name = "testName",
        };
        var responseCreated = await _client.PostAsJsonAsync("/Operation/operations", createdOperation);
        responseCreated.EnsureSuccessStatusCode();

        var createdOperationId = await responseCreated.Content.ReadAsStringAsync();

        var response = await _client.GetAsync($"/Operation/operations/id/{createdOperationId}");
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        var operation = await response.Content.ReadFromJsonAsync<Operation>();

        operation.Should().NotBeNull();
        operation.Should().BeEquivalentTo(new
        {
            Code = ranodmCode,
            Name = "testName"
        });

        return operation ?? new Operation();
    }
}