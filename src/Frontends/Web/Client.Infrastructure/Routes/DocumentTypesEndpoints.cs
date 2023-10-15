namespace BlazorApp.Client.Infrastructure.Routes
{
    public static class DocumentTypesEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return $"document/{ExportEndpoint}?searchString={searchString}";
        }

        public static string ExportEndpoint = "document/api/documentTypes/export";

        public static string Export(string searchString) => string.IsNullOrWhiteSpace(searchString)
                                                            ? ExportEndpoint
                                                            : ExportFiltered(searchString);

        public static string GetAll = "document/api/documentTypes";
        public static string Delete = "document/api/documentTypes";
        public static string Save = "document/api/documentTypes";
        public static string GetCount = "document/api/documentTypes/count";
    }
}