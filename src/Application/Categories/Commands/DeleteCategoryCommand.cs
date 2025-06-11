using Ardalis.Result;
using DecisionMate.Domain.Categories;
using Geneirodan.Abstractions.Repositories;
using Geneirodan.MediatR.Abstractions;
using MediatR;

namespace DecisionMate.Application.Categories.Commands;

public sealed record DeleteCategoryCommand(Guid Id) : ICommand
{
    public sealed class Handler(IRepository<Category, Guid> repository, IUnitOfWork unitOfWork)
        : IRequestHandler<DeleteCategoryCommand, Result>
    {
        public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await repository.FindAsync(request.Id, cancellationToken).ConfigureAwait(false);
            if (entity is null)
                return Result.NotFound();
            await repository.DeleteAsync(entity, cancellationToken).ConfigureAwait(false);
            await unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return Result.Success();
        }
    }
}