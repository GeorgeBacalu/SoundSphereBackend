using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Dtos.Response;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IFeedbackService
    {
        IList<FeedbackDto> GetAll(FeedbackPaginationRequest? payload);

        FeedbackDto GetById(Guid id);

        FeedbackDto Add(FeedbackDto feedbackDto, Guid userId);

        FeedbackDto UpdateById(FeedbackDto feedbackDto, Guid feedbackId, Guid userId);

        FeedbackDto DeleteById(Guid feedbackId, Guid userId);

        FeedbackStatisticsDto GetStatistics(DateTime? startDate, DateTime? endDate);
    }
}