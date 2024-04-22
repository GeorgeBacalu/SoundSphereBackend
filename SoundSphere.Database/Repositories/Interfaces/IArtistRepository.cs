using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Repositories.Interfaces
{
    public interface IArtistRepository
    {
        IList<Artist> FindAll();

        IList<Artist> FindAllActive();

        Artist FindById(Guid id);

        Artist Save(Artist artist);

        Artist UpdateById(Artist artist, Guid id);

        Artist DisableById(Guid id);

        void AddArtistLink(Artist artist);

        void AddUserArtist(Artist artist);
    }
}