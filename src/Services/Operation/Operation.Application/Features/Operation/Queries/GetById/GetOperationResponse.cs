namespace Operation.Application.Features.Operation.Queries.GetById;

public class GetOperationResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
}
