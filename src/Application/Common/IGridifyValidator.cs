using FluentValidation;
using Gridify;

namespace DecisionMate.Application.Common;

// ReSharper disable once UnusedTypeParameter
public interface IGridifyValidator<TEntity> : IValidator<GridifyQuery>;