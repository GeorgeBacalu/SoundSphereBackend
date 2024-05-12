using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IFeedbackService
    {
        IList<FeedbackDto> FindAll();

        IList<FeedbackDto> FindAllPagination(FeedbackPaginationRequest payload);

        FeedbackDto FindById(Guid id);

        FeedbackDto Save(FeedbackDto feedbackDto);

        FeedbackDto UpdateById(FeedbackDto feedbackDto, Guid id);

        void DeleteById(Guid id);
    }
}