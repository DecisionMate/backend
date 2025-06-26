using Ardalis.Result;
using Dapper;
using DecisionMate.Application.Categories;
using DecisionMate.Application.Categories.Mappers;
using DecisionMate.Application.Common;
using DecisionMate.Domain.Categories;
using DecisionMate.Domain.Categories.Enums;
using Geneirodan.Abstractions.Repositories;
using Geneirodan.EntityFrameworkCore;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace DecisionMate.Infrastructure.Categories;

[UsedImplicitly]
public sealed class CategoryRepository(DbContext context) : Repository<Category, Guid>(context), ICategoryRepository
{
    private readonly DbContext _context = context;

    public override Task<Category?> FindAsync(Guid id, CancellationToken token = default) =>
        base.FindAsync(Set.Include(x => x.Options), id, token);

    public async Task<Result<PageModel<CategoryListModel>>> GetCategories(string searchTerm,
        IPagination pagination,
        CategoryType type,
        CancellationToken cancellationToken)
    {
        var dbConnection = _context.Database.GetDbConnection();
        var command = CreateSearchCommand(searchTerm, pagination,type, cancellationToken);
        var models = await dbConnection.QueryAsync<CategoryListModel>(command).ConfigureAwait(false);
        var countCommand = CreateCountCommand(searchTerm, cancellationToken);
        return new PageModel<CategoryListModel>
        {
            Items = models.ToArray(),
            Page = pagination.Page,
            PageSize = pagination.PageSize,
            TotalCount = await dbConnection.ExecuteScalarAsync<int>(countCommand).ConfigureAwait(false)
        };
    }

    private static CommandDefinition CreateSearchCommand(
        string name,
        IPagination pagination,
        CategoryType type,
        CancellationToken cancellationToken
    ) =>
        new(
            commandText: """
                         SELECT id, name, description, image_url as imageUrl
                         FROM categories
                         WHERE type = @type AND name ILIKE (@name)
                         LIMIT @pageSize 
                         OFFSET @page
                         """,
            parameters: new
            {
                name = $"%{name}%",
                page = (pagination.Page - 1) * pagination.PageSize,
                pageSize = pagination.PageSize,
                type
            },
            cancellationToken: cancellationToken
        );

    private static CommandDefinition CreateCountCommand(string name, CancellationToken cancellationToken) =>
        new(
            commandText: """
                         SELECT COUNT(*) 
                         FROM categories
                         WHERE name ILIKE (@name)
                         """,
            parameters: new
            {
                name = $"%{name}%"
            },
            cancellationToken: cancellationToken
        );

    public async Task<CategoryModel?> GetCategory(Guid id, CancellationToken cancellationToken) =>
        await Set.AsNoTracking()
            .Where(x => x.Id.Equals(id))
            .Include(x => x.Options)
            .Select(x => x.MapToModel())
            .SingleOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
}