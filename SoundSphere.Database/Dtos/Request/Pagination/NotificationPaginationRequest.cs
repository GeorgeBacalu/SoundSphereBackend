using Microsoft.Data.SqlClient;
using SoundSphere.Database.Attributes;
using SoundSphere.Database.Dtos.Request.Models;
using SoundSphere.Database.Entities;
using System.ComponentModel.DataAnnotations;

namespace SoundSphere.Database.Dtos.Request.Pagination
{
    public record NotificationPaginationRequest(
        IDictionary<NotificationSortCriterion, SortOrder>? SortCriteria,
        IList<NotificationSearchCriterion>? SearchCriteria,

        [DateRange]
        DateTimeRange? DateRange,

        [StringLength(500, ErrorMessage = "Message can't be longer than 500 characters")]
        string? Message,

        [StringLength(75, ErrorMessage = "User name can't be longer than 75 characters")]
        string? UserName,
        
        bool? IsRead,
        
        NotificationType? Type) : PaginationRequest;

    public enum NotificationSortCriterion { InvalidSortCriterion, ByCreateDate = 10, ByMessage = 20, ByUserName = 30 }

    public enum NotificationSearchCriterion { InvalidSearchCriterion, ByCreateDateRange = 10, ByMessage = 20, ByUserName = 30, ByIsRead = 40, ByType = 50 }
}