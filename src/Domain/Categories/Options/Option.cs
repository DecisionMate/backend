using DecisionMate.Domain.Categories.Metadatas;
using DecisionMate.Domain.Categories.Options.ValueObjects;
using DecisionMate.Domain.Common;
using Geneirodan.Abstractions.Domain;

namespace DecisionMate.Domain.Categories.Options;

public sealed class Option : Entity<int>
{
    public OptionName Name { get; set; }
    public OptionDescription Description { get; set; }
    public Metadata? Metadata { get; init; }
    public Url? ImageUrl { get; set; }
}