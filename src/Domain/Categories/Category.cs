using DecisionMate.Domain.Categories.Options;
using DecisionMate.Domain.Categories.ValueObjects;
using DecisionMate.Domain.Common;
using Geneirodan.Abstractions.Domain;

namespace DecisionMate.Domain.Categories;

public sealed class Category : Entity<Guid>
{
    public CategoryName Name { get; set; }
    public CategoryDescription Description { get; set; }
    public Url? ImageUrl { get; set; }

    private HashSet<Option> _options = [];

    public IReadOnlySet<Option> Options
    {
        get => _options;
        set => _options = value.ToHashSet();
    }

}