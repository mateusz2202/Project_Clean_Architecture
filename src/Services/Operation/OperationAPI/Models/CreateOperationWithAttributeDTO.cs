namespace OperationAPI.Models;

public class CreateOperationWithAttributeDTO
{
    public CreateOperationDTO CreateOperationDTO { get; set; } = null!;
    public object? Attributes { get; set; } = null!;
}
