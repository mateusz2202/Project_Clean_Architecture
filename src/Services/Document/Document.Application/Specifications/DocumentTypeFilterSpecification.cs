using Document.Domain.Entities;

namespace Document.Application.Specifications;

public class DocumentTypeFilterSpecification : HeroSpecification<DocumentType>
{
    public DocumentTypeFilterSpecification(string searchString)
    {
        if (!string.IsNullOrEmpty(searchString))
        {
            Criteria = p => p.Name.Contains(searchString) || p.Description.Contains(searchString);
        }
        else
        {
            Criteria = p => true;
        }
    }
}
