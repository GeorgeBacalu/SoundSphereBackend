using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IFeedbackService
    {
        IList<FeedbackDto> GetAll();

        IList<FeedbackDto> GetAllPagination(FeedbackPaginationRequest payload);

        FeedbackDto GetById(Guid id);

        FeedbackDto Add(FeedbackDto feedbackDto);

        FeedbackDto UpdateById(FeedbackDto feedbackDto, Guid id);

        void DeleteById(Guid id);
    }
}