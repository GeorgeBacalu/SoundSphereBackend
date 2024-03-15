using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Repositories.Interfaces
{
    public interface IArtistRepository
    {
        IList<Artist> FindAll();

        Artist FindById(Guid id);

        Artist Save(Artist artist);

        Artist UpdateById(Artist artist, Guid id);

        Artist DisableById(Guid id);

        void AddArtistLink(Artist artist);

        void LinkArtistToUser(Artist artist);
    }
}