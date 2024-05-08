using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Extensions
{
    public static class UserQueryExtensions
    {
        public static IQueryable<User> Filter(this IQueryable<User> query, UserPaginationRequest payload)
        {
            if (payload.SearchCriteria == null || !payload.SearchCriteria.Any()) return query;
            return payload.SearchCriteria.Aggregate(query, (current, searchCriterion) => searchCriterion switch
            {
                UserSearchCriterion.ByName => current.Where(user => user.Name.Contains(payload.Name)),
                UserSearchCriterion.ByEmail => current.Where(user => user.Email.Contains(payload.Email)),
                UserSearchCriterion.ByBirthdayRange => current.Where(user => user.Birthday >= payload.DateRange.StartDate && user.Birthday <= payload.DateRange.EndDate),
                UserSearchCriterion.ByRole => current.Where(user => user.Role.Type.Equals(payload.RoleType)),
                _ => current
            });
        }

        public static IQueryable<User> Sort(this IQueryable<User> query, UserPaginationRequest payload)
        {
            if (payload.SortCriteria == null || !payload.SortCriteria.Any()) return query;
            return payload.SortCriteria.Aggregate(query, (current, sortCriterion) => sortCriterion.Key switch
            {
                UserSortCriterion.ByName => sortCriterion.Value == SortOrder.Ascending ? current.OrderBy(user => user.Name) : current.OrderByDescending(user => user.Name),
                UserSortCriterion.ByEmail => sortCriterion.Value == SortOrder.Ascending ? current.OrderBy(user => user.Email) : current.OrderByDescending(user => user.Email),
                _ => current
            });
        }

        public static IQueryable<User> Paginate(this IQueryable<User> query, UserPaginationRequest payload) => query.Skip((payload.Page - 1) * payload.Size).Take(payload.Size);
    }
}