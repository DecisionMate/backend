using System.Diagnostics.CodeAnalysis;
using DecisionMate.Application.Categories.Commands;
using DecisionMate.Application.Categories.Queries;
using DecisionMate.Application.Common;
using DecisionMate.Domain.Categories;
using DecisionMate.Domain.Categories.Enums;
using Geneirodan.Abstractions.Repositories;
using Geneirodan.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace DecisionMate.Web.Categories;

public static class CategoryEndpoints
{
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static IEndpointRouteBuilder MapCategoryEndpoints(
        this IEndpointRouteBuilder endpoints,
        [StringSyntax("Route")] string prefix
    )
    {
        var group = endpoints.MapGroup(prefix)
            .WithTags(nameof(Category))
            .WithDescription("Endpoints for the category")
            .ProducesValidationProblem();

        group.MapGet("/", GetCategories)
            .Produces<PageModel<CategoryListModel>>();

        group.MapPost("/", CreateCategory)
            .Produces<CategoryModel>(StatusCodes.Status201Created);

        var idGroup = group.MapGroup("/{id:guid}")
            .ProducesProblem(StatusCodes.Status404NotFound);

        idGroup.MapGet("/", GetCategory)
            .Produces<CategoryModel>();

        idGroup.MapPut("/", EditCategory)
            .Produces<CategoryModel>();

        idGroup.MapDelete("/", DeleteCategory)
            .Produces(StatusCodes.Status200OK);

        var templatesGroup = group.MapGroup("/templates");

        templatesGroup.MapGet("/", GetTemplates)
            .Produces<IReadOnlyCollection<CategoryTemplateModel>>();

        var templateTypeGroup = templatesGroup.MapGroup("/{type}");
        templateTypeGroup.MapGet("/", GetTemplate)
            .Produces<CategoryTemplateModel>();
        
        templateTypeGroup.MapGet("/category", GetPremadeCategory)
            .Produces<CategoryModel>();

        return group;
    }

    private static async Task<IResult> GetTemplates(
        [FromServices] ISender sender,
        CancellationToken token = default
    )
    {
        var query = new GetTemplatesQuery();
        var response = await sender.Send(query, token);
        return response.MapResult();
    }

    private static async Task<IResult> GetTemplate(
        [FromRoute] CategoryType type,
        [FromServices] ISender sender,
        CancellationToken token = default
    )
    {
        var query = new GetTemplateByTypeQuery(type);
        var response = await sender.Send(query, token);
        return response.MapResult();
    }

    private static async Task<IResult> GetPremadeCategory(
        HttpContext httpContext,
        [FromRoute] CategoryType type,
        [FromQuery] int count,
        [FromServices] ISender sender,
        CancellationToken token = default
    )
    {
        var filters = httpContext.Request.Query
            .ToDictionary(x => x.Key, x => x.Value.ToString(), StringComparer.OrdinalIgnoreCase);
        filters.Remove(nameof(count));
        
        var query = new GetPremadeCategoryByTypeQuery(type, count, filters);
        var response = await sender.Send(query, token);
        return response.MapResult();
    }

    private static async Task<IResult> GetCategory(
        [FromRoute] Guid id,
        [FromServices] ISender sender,
        CancellationToken token
    )
    {
        var query = new GetCategoryByIdQuery(id);
        var response = await sender.Send(query, token);
        return response.MapResult();
    }

    private static async Task<IResult> GetCategories(
        [FromServices] ISender sender,
        [FromQuery] string searchTerm = "",
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken token = default
    )
    {
        var query = new GetCustomCategoriesQuery(searchTerm, new Pagination(page, pageSize));
        var result = await sender.Send(query, token);
        return result.MapResult();
    }

    private static async Task<IResult> CreateCategory(
        [FromBody] AddCategoryRequest request,
        [FromServices] ISender sender,
        CancellationToken token
    )
    {
        var command = request.MapToCommand();
        var response = await sender.Send(command, token);
        return response.MapResult();
    }

    private static async Task<IResult> EditCategory(
        [FromRoute] Guid id,
        [FromBody] EditCategoryRequest request,
        [FromServices] ISender sender,
        CancellationToken token
    )
    {
        var command = request.MapToCommand(id);
        var response = await sender.Send(command, token);
        return response.MapResult();
    }

    private static async Task<IResult> DeleteCategory(
        [FromRoute] Guid id,
        [FromServices] ISender sender,
        CancellationToken token
    )
    {
        var command = new DeleteCategoryCommand(id);
        var response = await sender.Send(command, token);
        return response.MapResult();
    }
}