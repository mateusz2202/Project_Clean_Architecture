namespace Operation.Infrastructure.Configuration;

public class CosmosDbConfiguration
{
    public string DBName { get; set; } = string.Empty;
    public ComsosContaier[] Containers { get; set; } = Array.Empty<ComsosContaier>();
}

public record ComsosContaier(string Name, string Id, string PartitionKeyPath, int Throughput);
