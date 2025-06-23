namespace DecisionMate.Application.Common;

public interface IPagination
{
    int Page { get; }
    int PageSize { get; }
}