namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Routes
{
    public static class DocumentTypesEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return $"document/{Export}?searchString={searchString}";
        }

        public static string Export = "document/api/documentTypes/export";

        public static string GetAll = "document/api/documentTypes";
        public static string Delete = "document/api/documentTypes";
        public static string Save = "document/api/documentTypes";
        public static string GetCount = "document/api/documentTypes/count";
    }
}