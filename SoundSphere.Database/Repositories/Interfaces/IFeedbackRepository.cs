using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Repositories.Interfaces
{
    public interface IFeedbackRepository
    {
        IList<Feedback> FindAll();

        Feedback FindById(Guid id);

        Feedback Save(Feedback feedback);

        Feedback UpdateById(Feedback feedback, Guid id);

        void DeleteById(Guid id);

        void LinkFeedbackToUser(Feedback feedback);
    }
}