using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Operation.APIOperation.API_Test;
using System.Net.Http.Json;
using Xunit;
using Microsoft.AspNetCore.Authorization.Policy;
using Operation.Persistence;
using Operation.Application.Features.Operation.Commands.AddOperation;
using Operation.Application.Features.Operation.Queries.GetById;
using Operation.Shared.Wrapper;
using Operation.Application.Features.Operation.Queries.GetAll.Operations;

namespace Operation.API.Controllers.Tests;

public class OperationControllerTests
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public OperationControllerTests()
    {
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices((conf, services) =>
                {
                    services.AddMvc(options => options.Filters.Add(new FakeUserFilter()));
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

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
        var operations = (await response.Content.ReadFromJsonAsync<Result<List<GetAllOperationsResponse>>>());

        operations.Data.Should().NotBeNull();
        operations.Data.Should().HaveCountGreaterThan(9);

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

        var operation = await response.Content.ReadFromJsonAsync<Result<GetOperationResponse>>();

        operation.Data.Should().NotBeNull();
        operation.Data.Should().BeEquivalentTo(createdOperation);

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

    private async Task<GetOperationResponse> CreateOperation()
    {
        var ranodmCode = RandomGenerator.RandomString(4);

        var createdOperation = new AddOperationCommand()
        {
            Code = ranodmCode,
            Name = "testName",
        };
        var responseCreated = await _client.PostAsJsonAsync("/Operation/operations", createdOperation);
        responseCreated.EnsureSuccessStatusCode();

        var createdOperationResult = await responseCreated.Content.ReadFromJsonAsync<Result<int>>();

        var response = await _client.GetAsync($"/Operation/operations/id/{createdOperationResult.Data}");
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var operation = await response.Content.ReadFromJsonAsync<Result<GetOperationResponse>>();

        operation.Data.Should().NotBeNull();
        operation.Data.Should().BeEquivalentTo(new
        {
            Code = ranodmCode,
            Name = "testName"
        });

        return operation.Data ?? new GetOperationResponse();
    }

    [Fact]
    public async void UpdateOperationWithAttributesTest()
    {
        var createdOperation = await CreateOperation();

        createdOperation.Code = "teeeeest1";
        createdOperation.Name = "teeeeest2";

        var attribute = new
        {
            id = createdOperation.Id.ToString(),
            code = createdOperation.Code,
            height = "test1",
            width = "test2",
            weight = "test3",
        };

        var updateOperation = new { createOperationDTO = createdOperation, attributes = attribute };

        var responseCreatedAttribute = await _client.PutAsJsonAsync($"/Operation/id/{createdOperation.Id}", updateOperation);
        responseCreatedAttribute.EnsureSuccessStatusCode();

        var updatedObject = await _client.GetFromJsonAsync<Domain.Entities.Operation>($"/Operation/operations/id/{createdOperation.Id}");

        updatedObject.Should().NotBeNull();
        updatedObject.Should().BeEquivalentTo(createdOperation);

        var responseDelete = await _client.DeleteAsync($"/Operation/id/{createdOperation.Id}");

        responseDelete.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
    }
}