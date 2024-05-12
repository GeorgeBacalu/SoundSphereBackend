using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IArtistService
    {
        IList<ArtistDto> FindAll();

        IList<ArtistDto> FindAllActive();

        IList<ArtistDto> FindAllPagination(ArtistPaginationRequest payload);

        IList<ArtistDto> FindAllActivePagination(ArtistPaginationRequest payload);

        ArtistDto FindById(Guid id);

        ArtistDto Save(ArtistDto artistDto);

        ArtistDto UpdateById(ArtistDto artistDto, Guid id);

        ArtistDto DisableById(Guid id);
    }
}