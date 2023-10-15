using System.Text.Json;
using BlazorApp.Application.Interfaces.Serialization.Options;

namespace BlazorApp.Application.Serialization.Options;

public class SystemTextJsonOptions : IJsonSerializerOptions
{
    public JsonSerializerOptions JsonSerializerOptions { get; } = new();
}