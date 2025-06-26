using DecisionMate.Domain.Categories.Enums;
using DecisionMate.Domain.Categories.Events;
using DecisionMate.Domain.Categories.Options;
using DecisionMate.Domain.Categories.ValueObjects;
using DecisionMate.Domain.Common;

namespace DecisionMate.Domain.Categories;

public sealed class Category : AggregateRoot
{
    private CategoryDescription? _description;
    private Url? _imageUrl;
    private CategoryName _name;

    private HashSet<Option> _options = [];

    public CategoryType Type { get; init; }
    
    public CategoryName Name
    {
        get => _name;
        init => _name = value;
    }

    public CategoryDescription? Description
    {
        get => _description;
        init => _description = value;
    }

    public Url? ImageUrl
    {
        get => _imageUrl;
        init => _imageUrl = value;
    }

    public IReadOnlySet<Option> Options
    {
        get => _options;
        init => _options = value.ToHashSet();
    }

    public static (Category category, CategoryCreatedEvent @event) Create(
        CategoryName name,
        CategoryDescription description,
        CategoryType type,
        IReadOnlySet<Option> options,
        Url? imageUrl
    )
    {
        var category = new Category
        {
            Id = Guid.CreateVersion7(),
            Name = name,
            Description = description,
            Type = type,
            ImageUrl = imageUrl,
            Options = options
        };
        var @event = new CategoryCreatedEvent(category.Id, category.Name, category.Description, category.ImageUrl);
        category.AddEvent(@event);
        return (category, @event);
    }

    public CategoryUpdatedEvent Edit(
        CategoryName name,
        CategoryDescription description,
        IReadOnlySet<Option> options,
        Url? imageUrl
    )
    {
        _name = name;
        _description = description;
        _imageUrl = imageUrl;
        _options = options.ToHashSet();
        var @event = new CategoryUpdatedEvent(Id, Name, Description, ImageUrl);
        AddEvent(@event);
        return @event;
    }

    public CategoryDeletedEvent Delete()
    {
        IsDeleted = true;
        var @event = new CategoryDeletedEvent(Id);
        AddEvent(@event);
        return @event;
    }
}