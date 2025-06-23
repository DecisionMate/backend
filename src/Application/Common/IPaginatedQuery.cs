using Geneirodan.Abstractions.Repositories;
using Geneirodan.MediatR.Abstractions;

namespace DecisionMate.Application.Common;

public interface IPaginatedQuery<T> : IQuery<PageModel<T>>
{
    IPagination Pagination { get; }
}