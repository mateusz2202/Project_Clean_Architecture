using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Application.Features.DocumentTypes;

public class AddEditDocumentTypeCommand
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
}

