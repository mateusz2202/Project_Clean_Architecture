using BlazorApp.Application.Interfaces.Serialization.Settings;
using Newtonsoft.Json;

namespace BlazorApp.Application.Serialization.Settings;

public class NewtonsoftJsonSettings : IJsonSerializerSettings
{
    public JsonSerializerSettings JsonSerializerSettings { get; } = new();
}