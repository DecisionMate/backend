using DecisionMate.Application.Categories.Mappers;
using DecisionMate.Domain.Categories;
using Geneirodan.Abstractions.Mapping;
using JetBrains.Annotations;

namespace DecisionMate.Infrastructure.Categories;

[UsedImplicitly]
public sealed class CategoryModelMapper : IEntityMapper<Category, CategoryModel>
{
    public CategoryModel Map(Category source) => source.MapToModel();
    
    public IQueryable<CategoryModel> Map(IQueryable<Category> source) => source.Select(x => x.MapToModel());
}