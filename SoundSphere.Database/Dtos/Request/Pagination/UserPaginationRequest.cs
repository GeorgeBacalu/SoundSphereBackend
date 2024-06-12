using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Request.Models;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Dtos.Request.Pagination
{
    public record UserPaginationRequest(
        IDictionary<UserSortCriterion, SortOrder>? SortCriteria,
        IList<UserSearchCriterion>? SearchCriteria,
        string? Name,
        string? Email,
        DateRange? DateRange,
        RoleType? RoleType) : PaginationRequest;

    public enum UserSortCriterion { InvalidSortCriterion, ByName = 10, ByEmail = 20 }

    public enum UserSearchCriterion { InvalidSearchCriterion, ByName = 10, ByEmail = 20, ByBirthdayRange = 30, ByRole = 40 }
}