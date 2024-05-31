using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IArtistService
    {
        IList<ArtistDto> GetAll();

        IList<ArtistDto> GetAllActive();

        IList<ArtistDto> GetAllPagination(ArtistPaginationRequest payload);

        IList<ArtistDto> GetAllActivePagination(ArtistPaginationRequest payload);

        ArtistDto GetById(Guid id);

        ArtistDto Add(ArtistDto artistDto);

        ArtistDto UpdateById(ArtistDto artistDto, Guid id);

        ArtistDto DeleteById(Guid id);
    }
}