namespace BlazorHero.CleanArchitecture.Client.Infrastructure.Routes
{
    public static class ExtendedAttributesEndpoints
    {
        public static string GetAll(string entityName) => $"document/api/{entityName}ExtendedAttributes";
        public static string GetAllByEntityId<TEntityId>(string entityName, TEntityId entityId) => $"document/{GetAll(entityName)}/by-entity/{entityId}";
        public static string Export<TEntityId>(string entityName, TEntityId entityId, bool includeEntity, bool onlyCurrentGroup = false, string currentGroup = "") => $"api/{entityName}ExtendedAttributes/export?{nameof(entityId)}={entityId}&{nameof(includeEntity)}={includeEntity}&{nameof(onlyCurrentGroup)}={onlyCurrentGroup}&{nameof(currentGroup)}={currentGroup}";
        public static string ExportFiltered<TEntityId>(string entityName, string searchString, TEntityId entityId, bool includeEntity, bool onlyCurrentGroup = false, string currentGroup = "") => $"api/{entityName}ExtendedAttributes/export?{nameof(searchString)}={searchString}&{nameof(entityId)}={entityId}&{nameof(includeEntity)}={includeEntity}&{nameof(onlyCurrentGroup)}={onlyCurrentGroup}&{nameof(currentGroup)}={currentGroup}";
        public static string Delete(string entityName) => $"document/api/{entityName}ExtendedAttributes";
        public static string Save(string entityName) => $"document/api/{entityName}ExtendedAttributes";
        public static string GetCount(string entityName) => $"document/api/{entityName}ExtendedAttributes/count";
    }
}