using Ardalis.Result;
using DecisionMate.Application.Categories;
using DecisionMate.Domain.Categories;
using Geneirodan.Abstractions.Mapping;
using Geneirodan.Abstractions.Repositories;
using Geneirodan.EntityFrameworkCore;
using Gridify;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace DecisionMate.Infrastructure.Categories;

[UsedImplicitly]
public sealed class CategoryRepository(
    DbContext context,
    IEntityMapper<Category, CategoryListModel> listMapper,
    IGridifyMapper<Category> gridifyMapper,
    IEntityMapper<Category, CategoryModel> viewModelMapper
) : Repository<Category, Guid>(context), ICategoryRepository
{
    public override Task<Category?> FindAsync(Guid id, CancellationToken token = default) =>
        base.FindAsync(Set.Include(x => x.Options), id, token);

    public async Task<Result<PageModel<CategoryListModel>>> GetCategories(
        GridifyQuery query, 
        CancellationToken cancellationToken
        )
    {
        var queryable = Set.GridifyQueryable(query, gridifyMapper);
        var entities = await listMapper.Map(queryable.Query).ToArrayAsync(cancellationToken).ConfigureAwait(false);
        return new PageModel<CategoryListModel>
        {
            Items = entities,
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = queryable.Count
        };
    }

    public async Task<CategoryModel?> GetCategory(Guid id, CancellationToken cancellationToken) =>
        await Set.AsNoTracking()
            .Where(x => x.Id.Equals(id))
            .Include(x => x.Options)
            .ProjectTo<Category, CategoryModel>(viewModelMapper)
            .SingleOrDefaultAsync(cancellationToken).ConfigureAwait(false);

}