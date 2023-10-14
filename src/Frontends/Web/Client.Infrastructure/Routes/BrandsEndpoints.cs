namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Routes;

public static class BrandsEndpoints
{
    public static string ExportFiltered(string searchString)
    {
        return $"product/{Export}?searchString={searchString}";
    }

    private static string ExportEndpoint = "product/api/brands/export";
    public static string Export(string searchString) => string.IsNullOrWhiteSpace(searchString)
                                                       ? ExportEndpoint
                                                       : ExportFiltered(searchString);

    public static string GetAll = "product/api/brands";
    public static string Delete = "product/api/brands";
    public static string Save = "product/api/brands";
    public static string GetCount = "product/api/brands/count";
    public static string Import = "product/api/brands/import";
}