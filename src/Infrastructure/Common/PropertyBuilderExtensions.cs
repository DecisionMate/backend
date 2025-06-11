using System.Text.Json;
using DecisionMate.Domain.Common;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DecisionMate.Infrastructure.Common;

[PublicAPI]
public static class PropertyBuilderExtensions
{
    public static PropertyBuilder<Url?> HasUrlToStringConversion(this PropertyBuilder<Url?> propertyBuilder) =>
        propertyBuilder.HasConversion(
            x => x == null ? null : x.Value.Value,
            x => x == null ? null : new Url(x)
        );

    public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder) =>
        propertyBuilder.HasConversion(
            x => JsonSerializer.Serialize(x, null as JsonSerializerOptions),
            x => JsonSerializer.Deserialize<T>(x, null as JsonSerializerOptions)!
        );
}