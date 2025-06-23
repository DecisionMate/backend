namespace DecisionMate.Application.Common;

public record Pagination(int Page, int PageSize) : IPagination;