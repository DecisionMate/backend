using DecisionMate.Application.Categories.Commands;
using DecisionMate.Application.Categories.Contracts;
using DecisionMate.Domain.Categories;
using DecisionMate.Domain.Categories.Options;
using Riok.Mapperly.Abstractions;
using static Riok.Mapperly.Abstractions.RequiredMappingStrategy;

namespace DecisionMate.Application.Categories.Mappers;

[Mapper]
[UseStaticMapper(typeof(OptionMapper))]
public static partial class CategoryMapper
{
    [MapperRequiredMapping(Target)]
    [MapValue(nameof(Category.Id), Use = nameof(CreateId))]
    public static partial Category MapToCategory(this CreateCategoryCommand command);


    [MapperRequiredMapping(Source)]
    [MapperIgnoreSource(nameof(EditCategoryCommand.Id))]
    public static partial void MapToCategory(this EditCategoryCommand command, Category category);

    [MapperRequiredMapping(Target)]
    public static partial CategoryModel MapToModel(this Category category);

    private static Guid CreateId() => Guid.CreateVersion7();

#pragma warning disable CA1859
    [UserMapping]
    private static IReadOnlySet<Option> Map(this OptionContract[] options) =>
        options.Select((x, i) => x.Map(i + 1)).ToHashSet();
#pragma warning restore CA1859

    [UserMapping]
    private static OptionModel[] Map(this IReadOnlySet<Option> options) =>
        options.OrderBy(x => x.Id).Select(OptionMapper.Map).ToArray();
}