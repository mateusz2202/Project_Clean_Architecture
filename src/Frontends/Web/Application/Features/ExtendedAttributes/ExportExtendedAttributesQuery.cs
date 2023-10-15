namespace BlazorApp.Application.Features.ExtendedAttributes;

public record ExportExtendedAttributesQuery<TId, TEntityId, TEntity, TExtendedAttribute>
(string SearchString = "", TEntityId EntityId = default, bool IncludeEntity = false, bool OnlyCurrentGroup = false, string CurrentGroup = "");  
   
