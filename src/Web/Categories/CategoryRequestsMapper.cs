using DecisionMate.Application.Categories.Commands;
using Riok.Mapperly.Abstractions;

namespace DecisionMate.Web.Categories;

[Mapper]
internal static partial class CategoryRequestsMapper
{
    [MapperRequiredMapping(RequiredMappingStrategy.Target)]
    public static partial CreateCategoryCommand MapToCommand(this AddCategoryRequest request);
    
    [MapperRequiredMapping(RequiredMappingStrategy.Target)]
    public static partial EditCategoryCommand MapToCommand(this EditCategoryRequest request, Guid id);
}