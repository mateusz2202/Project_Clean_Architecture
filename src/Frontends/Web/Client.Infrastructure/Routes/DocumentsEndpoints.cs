namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Routes
{
    public static class DocumentsEndpoints
    {
        public static string GetAllPaged(int pageNumber, int pageSize, string searchString)
        {
            return $"document/api/documents?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}";
        }

        public static string GetById(int documentId)
        {
            return $"document/api/documents/{documentId}";
        }

        public static string Save = "document/api/documents";
        public static string Delete = "document/api/documents";
    }
}