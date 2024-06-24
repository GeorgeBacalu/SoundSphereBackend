using Microsoft.Data.SqlClient;
using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Extensions
{
    public static class UserQueryExtensions
    {
        public static IQueryable<User> Filter(this IQueryable<User> query, UserPaginationRequest payload)
        {
            if (payload.SearchCriteria == null || !payload.SearchCriteria.Any())
                return query;
            foreach (var searchCriterion in payload.SearchCriteria)
                query = searchCriterion switch
                {
                    UserSearchCriterion.ByName => query.Where(user => user.Name.Contains(payload.Name)),
                    UserSearchCriterion.ByEmail => query.Where(user => user.Email.Contains(payload.Email)),
                    UserSearchCriterion.ByBirthdayRange => query.Where(user => user.Birthday >= payload.DateRange.StartDate && user.Birthday <= payload.DateRange.EndDate),
                    UserSearchCriterion.ByRole => query.Where(user => user.Role.Type.Equals(payload.RoleType)),
                    _ => query
                };
            return query;
        }

        public static IQueryable<User> Sort(this IQueryable<User> query, UserPaginationRequest payload)
        {
            if (payload.SortCriteria == null || !payload.SortCriteria.Any())
                return query.OrderBy(user => user.CreatedAt);
            var firstCriterion = payload.SortCriteria.First();
            var orderedQuery = firstCriterion.Key switch
            {
                UserSortCriterion.ByName => firstCriterion.Value == SortOrder.Ascending ? query.OrderBy(user => user.Name) : query.OrderByDescending(user => user.Name),
                UserSortCriterion.ByEmail => firstCriterion.Value == SortOrder.Ascending ? query.OrderBy(user => user.Email) : query.OrderByDescending(user => user.Email),
                _ => query.OrderBy(user => user.CreatedAt)
            };
            foreach (var sortCriterion in payload.SortCriteria.Skip(1))
                orderedQuery = sortCriterion.Key switch
                {
                    UserSortCriterion.ByName => sortCriterion.Value == SortOrder.Ascending ? orderedQuery.ThenBy(user => user.Name) : orderedQuery.ThenByDescending(user => user.Name),
                    UserSortCriterion.ByEmail => sortCriterion.Value == SortOrder.Ascending ? orderedQuery.ThenBy(user => user.Email) : orderedQuery.ThenByDescending(user => user.Email),
                    _ => orderedQuery
                };
            return orderedQuery;
        }

        public static IQueryable<User> Paginate(this IQueryable<User> query, UserPaginationRequest payload) => query.Skip(payload.Page * payload.Size).Take(payload.Size);

        public static IQueryable<User> ApplyPagination(this IQueryable<User> query, UserPaginationRequest? payload) => payload == null ? query : query.Filter(payload).Sort(payload).Paginate(payload);
    }
}