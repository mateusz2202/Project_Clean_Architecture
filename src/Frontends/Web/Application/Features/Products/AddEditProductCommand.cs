using System.ComponentModel.DataAnnotations;
using BlazorApp.Application.Requests;

namespace BlazorApp.Application.Features.Products;

public class AddEditProductCommand
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Barcode { get; set; }
    [Required]
    public string Description { get; set; }
    public string ImageDataURL { get; set; }
    [Required]
    public decimal Rate { get; set; }
    [Required]
    public int BrandId { get; set; }
    public UploadRequest UploadRequest { get; set; }
}
