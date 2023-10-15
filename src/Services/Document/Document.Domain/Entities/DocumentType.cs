using Document.Domain.Contracts;

namespace Document.Domain.Entities;

public class DocumentType : AuditableEntity<int>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}