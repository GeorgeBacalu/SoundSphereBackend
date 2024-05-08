using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Repositories.Interfaces
{
    public interface IFeedbackRepository
    {
        IList<Feedback> FindAll();

        IList<Feedback> FindAllPagination(FeedbackPaginationRequest payload);

        Feedback FindById(Guid id);

        Feedback Save(Feedback feedback);

        Feedback UpdateById(Feedback feedback, Guid id);

        void DeleteById(Guid id);

        void LinkFeedbackToUser(Feedback feedback);
    }
}