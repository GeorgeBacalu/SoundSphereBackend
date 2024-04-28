using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IFeedbackService
    {
        IList<FeedbackDto> FindAll();

        FeedbackDto FindById(Guid id);

        FeedbackDto Save(FeedbackDto feedbackDto);

        FeedbackDto UpdateById(FeedbackDto feedbackDto, Guid id);

        void DeleteById(Guid id);

        IList<FeedbackDto> ConvertToDtos(IList<Feedback> feedbacks);

        IList<Feedback> ConvertToEntities(IList<FeedbackDto> feedbackDtos);

        FeedbackDto ConvertToDto(Feedback feedback);

        Feedback ConvertToEntity(FeedbackDto feedbackDto);
    }
}