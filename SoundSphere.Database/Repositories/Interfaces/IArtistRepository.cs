using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Repositories.Interfaces
{
    public interface IArtistRepository
    {
        IList<Artist> GetAll();

        IList<Artist> GetAllActive();

        IList<Artist> GetAllPagination(ArtistPaginationRequest payload);

        IList<Artist> GetAllActivePagination(ArtistPaginationRequest payload);

        Artist GetById(Guid id);

        Artist Add(Artist artist);

        Artist UpdateById(Artist artist, Guid id);

        Artist DeleteById(Guid id);

        void AddArtistLink(Artist artist);

        void AddUserArtist(Artist artist);
    }
}