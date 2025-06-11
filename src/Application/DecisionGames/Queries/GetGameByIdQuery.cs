using Ardalis.Result;
using DecisionMate.Domain.DecisionGames.Models;
using Geneirodan.MediatR.Abstractions;
using MediatR;

namespace DecisionMate.Application.DecisionGames.Queries;

public sealed record GetGameByIdQuery(Guid Id) : IQuery<DecisionGameModel>
{
    public sealed class Handler(IDecisionGameRepository repository)
        : IRequestHandler<GetGameByIdQuery, Result<DecisionGameModel>>
    {
        public async Task<Result<DecisionGameModel>> Handle(
            GetGameByIdQuery request,
            CancellationToken cancellationToken
        ) => await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false) is {} model 
             ? Result<DecisionGameModel>.Success(model)
             : Result<DecisionGameModel>.NotFound();
    }
}