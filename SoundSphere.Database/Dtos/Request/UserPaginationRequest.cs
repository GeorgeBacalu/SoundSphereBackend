using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Request.Models;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Dtos.Request
{
    public class UserPaginationRequest : PaginationRequest
    {
        public IDictionary<UserSortCriterion, SortOrder>? SortCriteria { get; set; }
        public IList<UserSearchCriterion>? SearchCriteria { get; set; }
        public string? Name { get; set; }
        public string? Email {  get; set; }
        public DateRange? DateRange { get; set; }
        public RoleType RoleType { get; set; }
    }

    public enum UserSortCriterion { ByName = 10, ByEmail = 20 }

    public enum UserSearchCriterion { ByName = 10, ByEmail = 20, ByBirthdayRange = 30, ByRole = 40 }
}