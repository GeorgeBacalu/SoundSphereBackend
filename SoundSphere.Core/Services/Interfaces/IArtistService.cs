using SoundSphere.Database.Dtos;
using SoundSphere.Database.Entities;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IArtistService
    {
        IList<ArtistDto> FindAll();

        IList<ArtistDto> FindAllActive();

        ArtistDto FindById(Guid id);

        ArtistDto Save(ArtistDto artistDto);

        ArtistDto UpdateById(ArtistDto artistDto, Guid id);

        ArtistDto DisableById(Guid id);

        IList<ArtistDto> ConvertToDtos(IList<Artist> artists);

        IList<Artist> ConvertToEntities(IList<ArtistDto> artistDtos);

        ArtistDto ConvertToDto(Artist artist);

        Artist ConvertToEntity(ArtistDto artistDto);
    }
}