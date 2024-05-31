using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Request.Models;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Dtos.Request
{
    public record UserPaginationRequest(
        IDictionary<UserSortCriterion, SortOrder>? SortCriteria,
        IList<UserSearchCriterion>? SearchCriteria,
        string? Name,
        string? Email,
        DateRange? DateRange,
        RoleType RoleType
        ) : PaginationRequest;

    public enum UserSortCriterion { ByName = 10, ByEmail = 20 }

    public enum UserSearchCriterion { ByName = 10, ByEmail = 20, ByBirthdayRange = 30, ByRole = 40 }
}