using BlazorHero.CleanArchitecture.Application.Requests;

namespace BlazorHero.CleanArchitecture.Application.Features.Brands;

public partial class ImportBrandsCommand
{
    public UploadRequest UploadRequest { get; set; }
}

