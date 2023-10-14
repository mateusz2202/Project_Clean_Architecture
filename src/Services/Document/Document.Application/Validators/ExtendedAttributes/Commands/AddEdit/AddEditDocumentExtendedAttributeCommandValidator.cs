using Document.Domain.ExtendedAttributes;

namespace Document.Application.Validators.ExtendedAttributes.Commands.AddEdit;

public class AddEditDocumentExtendedAttributeCommandValidator : AddEditExtendedAttributeCommandValidator<int, int, Domain.Entities.Document, DocumentExtendedAttribute>
{
    public AddEditDocumentExtendedAttributeCommandValidator() : base()
    {
        // you can override the validation rules here
    }
}
