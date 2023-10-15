using Microsoft.AspNetCore.Components;

namespace BlazorApp.Client.Shared.Components;

public abstract partial class ExtendedAttributes<TId, TEntityId, TEntity, TExtendedAttribute>
    : ExtendedAttributesBase<TId, TEntityId, TEntity, TExtendedAttribute>          
{
    protected override RenderFragment Inherited() => builder => base.BuildRenderTree(builder);
}