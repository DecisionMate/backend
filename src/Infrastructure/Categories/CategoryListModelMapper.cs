using DecisionMate.Domain.Categories;
using Geneirodan.Abstractions.Mapping;
using JetBrains.Annotations;
using Riok.Mapperly.Abstractions;

namespace DecisionMate.Infrastructure.Categories;

[Mapper, UsedImplicitly]
public sealed partial class CategoryListModelMapper : IEntityMapper<Category, CategoryListModel>
{
    [MapperRequiredMapping(RequiredMappingStrategy.Target)]
    public partial CategoryListModel Map(Category source);

    public partial IQueryable<CategoryListModel> Map(IQueryable<Category> source);
}