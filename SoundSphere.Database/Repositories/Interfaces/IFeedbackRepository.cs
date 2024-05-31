using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Repositories.Interfaces
{
    public interface IFeedbackRepository
    {
        IList<Feedback> GetAll();

        IList<Feedback> GetAllPagination(FeedbackPaginationRequest payload);

        Feedback GetById(Guid id);

        Feedback Add(Feedback feedback);

        Feedback UpdateById(Feedback feedback, Guid id);

        void DeleteById(Guid id);

        void LinkFeedbackToUser(Feedback feedback);
    }
}