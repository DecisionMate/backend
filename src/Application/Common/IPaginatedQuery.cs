using Geneirodan.Abstractions.Repositories;
using Geneirodan.MediatR.Abstractions;
using JetBrains.Annotations;

namespace DecisionMate.Application.Common;

[PublicAPI]
public interface IPaginatedQuery<T> : IQuery<PageModel<T>>
{
    IPagination Pagination { get; }
}