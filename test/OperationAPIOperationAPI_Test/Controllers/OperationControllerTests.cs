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
using Operation.Application.Features.Operation.Commands.AddAtributes;
using System.Dynamic;
using Operation.Application.Features.Operation.Commands.UpdateOperationWithattribute;

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

        var addOperationAttributCommad = new AddOperationAttributCommad(attribute);


        var responseCreatedAttribute = await _client.PostAsJsonAsync("/Operation/attributes", addOperationAttributCommad);
        responseCreatedAttribute.EnsureSuccessStatusCode();

        var response = await _client.GetAsync("/Operation");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var operations = await response.Content.ReadFromJsonAsync<Result<List<ExpandoObject>>>();

        operations.Data.Should().NotBeNull();
        operations.Data.Should().HaveCountGreaterThan(0);

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

        var addOperationAttributCommad = new AddOperationAttributCommad(attribute);

        var responseCreatedAttribute = await _client.PostAsJsonAsync("/Operation/attributes", addOperationAttributCommad);
        responseCreatedAttribute.EnsureSuccessStatusCode();

        var response = await _client.GetAsync("/Operation/attributes");
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var operations = await response.Content.ReadFromJsonAsync<Result<List<ExpandoObject>>>();

        operations.Data.Should().NotBeNull();
        operations.Data.Should().HaveCountGreaterThan(0);

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

        var createdOperation = new AddOperationCommand(ranodmCode, "testName");

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
        var newCode = RandomGenerator.RandomString(6);
        var attribute = new
        {
            id = createdOperation.Id.ToString(),
            code = newCode,
            height = "test1",
            width = "test2",
            weight = "test3",
        };

        var updateOperationWithAttributesCommand = new UpdateOperationWithAttributesCommand
            (
                createdOperation.Id,
                new AddOperationCommand(newCode, createdOperation.Name),
                attribute
            );

        var responseCreatedAttribute = await _client.PutAsJsonAsync($"/Operation/", updateOperationWithAttributesCommand);
        responseCreatedAttribute.EnsureSuccessStatusCode();

        var updatedObject = await _client.GetFromJsonAsync<Result<GetOperationResponse>>($"/Operation/operations/id/{createdOperation.Id}");

        updatedObject.Data.Should().NotBeNull();
        createdOperation.Code = newCode;
        updatedObject.Data.Should().BeEquivalentTo(createdOperation);

        var responseDelete = await _client.DeleteAsync($"/Operation/id/{createdOperation.Id}");

        responseDelete.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
    }
}