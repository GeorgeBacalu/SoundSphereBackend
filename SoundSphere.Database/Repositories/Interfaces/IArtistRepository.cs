using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Repositories.Interfaces
{
    public interface IArtistRepository
    {
        IList<Artist> FindAll();

        IList<Artist> FindAllActive();

        IList<Artist> FindAllPagination(ArtistPaginationRequest payload);

        IList<Artist> FindAllActivePagination(ArtistPaginationRequest payload);

        Artist FindById(Guid id);

        Artist Save(Artist artist);

        Artist UpdateById(Artist artist, Guid id);

        Artist DisableById(Guid id);

        void AddArtistLink(Artist artist);

        void AddUserArtist(Artist artist);
    }
}