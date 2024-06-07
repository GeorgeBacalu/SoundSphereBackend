using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Entities;

namespace SoundSphere.Database.Repositories.Interfaces
{
    public interface IArtistRepository
    {
        IList<Artist> GetAll(ArtistPaginationRequest payload);

        Artist GetById(Guid id);

        Artist Add(Artist artist);

        Artist UpdateById(Artist artist, Guid id);

        Artist DeleteById(Guid id);

        void AddArtistLink(Artist artist);

        void AddUserArtist(Artist artist);
    }
}