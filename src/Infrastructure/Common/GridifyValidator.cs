using DecisionMate.Application.Common;
using FluentValidation;
using Gridify;
using JetBrains.Annotations;

namespace DecisionMate.Infrastructure.Common;

[UsedImplicitly]
public sealed class GridifyValidator<T> : AbstractValidator<GridifyQuery>, IGridifyValidator<T>
{
    public GridifyValidator(IGridifyMapper<T> mapper) =>
        RuleFor(x => x).NotEmpty().Must(x => x.IsValid(mapper));
}