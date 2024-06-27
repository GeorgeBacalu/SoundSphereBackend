using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Repositories.Interfaces
{
    public interface IFeedbackRepository
    {
        IList<Feedback> GetAll(FeedbackPaginationRequest? payload);

        Feedback GetById(Guid id);

        Feedback Add(Feedback feedback);

        Feedback UpdateById(Feedback feedback, Guid id);

        Feedback DeleteById(Guid id);

        int CountByDateRangeAndType(DateTime? startDate, DateTime? endDate, FeedbackType? type);

        void LinkFeedbackToUser(Feedback feedback);
    }
}