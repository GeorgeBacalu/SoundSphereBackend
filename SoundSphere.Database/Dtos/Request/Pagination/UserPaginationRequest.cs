using Microsoft.Data.SqlClient;
using SoundSphere.Database.Attributes;
using SoundSphere.Database.Dtos.Request.Models;
using SoundSphere.Database.Entities;
using System.ComponentModel.DataAnnotations;

namespace SoundSphere.Database.Dtos.Request.Pagination
{
    public record UserPaginationRequest(
        IDictionary<UserSortCriterion, SortOrder>? SortCriteria,
        IList<UserSearchCriterion>? SearchCriteria,

        [StringLength(75, ErrorMessage = "Name can't be longer than 75 characters")]
        string? Name,

        [StringLength(75, ErrorMessage = "Email can't be longer than 75 characters")]
        string? Email,

        [DateRange]
        DateRange? DateRange,
        
        RoleType? RoleType) : PaginationRequest;

    public enum UserSortCriterion { InvalidSortCriterion, ByName = 10, ByEmail = 20 }

    public enum UserSearchCriterion { InvalidSearchCriterion, ByName = 10, ByEmail = 20, ByBirthdayRange = 30, ByRole = 40 }
}