using DecisionMate.Application.Categories.Contracts;
using DecisionMate.Domain.Categories;
using DecisionMate.Domain.Categories.Options;
using Riok.Mapperly.Abstractions;
using static Riok.Mapperly.Abstractions.RequiredMappingStrategy;

namespace DecisionMate.Application.Categories.Mappers;

[Mapper]
[UseStaticMapper(typeof(OptionMappingExtensions))]
public static partial class CategoryMappingExtensions
{
    [MapperRequiredMapping(Target)]
    public static partial CategoryModel MapToModel(this Category category);
    
    [UserMapping]
    public static IReadOnlySet<Option> Map(this OptionContract[] options) =>
        options.Select((x, i) => x.Map(i + 1)).ToHashSet();

    [UserMapping]
    public static OptionModel[] Map(this IReadOnlySet<Option> options) =>
        options.OrderBy(x => x.Id).Select(OptionMappingExtensions.Map).ToArray();
}