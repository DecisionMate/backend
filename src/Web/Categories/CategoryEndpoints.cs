using System.Diagnostics.CodeAnalysis;
using DecisionMate.Application.Categories.Commands;
using DecisionMate.Application.Categories.Queries;
using DecisionMate.Domain.Categories;
using Geneirodan.Abstractions.Repositories;
using Geneirodan.AspNetCore;
using Gridify;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

        return group;
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
        [AsParameters] GridifyQuery parameters,
        [FromServices] ISender sender,
        CancellationToken token
    )
    {
        var query = new GetCategoriesQuery(parameters);
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