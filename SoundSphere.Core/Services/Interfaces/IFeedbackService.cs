using SoundSphere.Database.Entities;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IFeedbackService
    {
        IList<Feedback> FindAll();

        Feedback FindById(Guid id);

        Feedback Save(Feedback feedback);

        Feedback UpdateById(Feedback feedback, Guid id);

        void DeleteById(Guid id);
    }
}