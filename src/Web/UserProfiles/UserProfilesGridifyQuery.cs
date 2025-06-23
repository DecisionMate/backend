using DecisionMate.Application.Common;
using JetBrains.Annotations;

namespace DecisionMate.Web.UserProfiles;

[UsedImplicitly]
internal record UserProfilesGridifyQuery : IPagination
{
    public int Page { get; set; }
    public int PageSize { get; set; }
}