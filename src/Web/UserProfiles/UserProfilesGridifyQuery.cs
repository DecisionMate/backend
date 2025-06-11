using Gridify;
using JetBrains.Annotations;

namespace DecisionMate.Web.UserProfiles;

[UsedImplicitly]
internal record UserProfilesGridifyQuery : IGridifyPagination
{
    public int Page { get; set; }
    public int PageSize { get; set; }
}