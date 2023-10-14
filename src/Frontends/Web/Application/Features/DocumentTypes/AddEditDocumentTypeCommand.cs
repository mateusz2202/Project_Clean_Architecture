using System.ComponentModel.DataAnnotations;

namespace BlazorHero.CleanArchitecture.Application.Features.DocumentTypes;

public class AddEditDocumentTypeCommand
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
}

