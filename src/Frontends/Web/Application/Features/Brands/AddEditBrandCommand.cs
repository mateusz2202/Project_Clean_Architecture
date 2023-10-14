using System.ComponentModel.DataAnnotations;

namespace BlazorHero.CleanArchitecture.Application.Features.Brands;

public partial class AddEditBrandCommand
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public decimal Tax { get; set; }
}

