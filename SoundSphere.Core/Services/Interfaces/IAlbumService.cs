using SoundSphere.Database.Dtos;
using SoundSphere.Database.Entities;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IAlbumService
    {
        IList<AlbumDto> FindAll();

        IList<AlbumDto> FindAllActive();

        AlbumDto FindById(Guid id);

        AlbumDto Save(AlbumDto albumDto);

        AlbumDto UpdateById(AlbumDto albumDto, Guid id);

        AlbumDto DisableById(Guid id);

        IList<AlbumDto> ConvertToDtos(IList<Album> albums);

        IList<Album> ConvertToEntities(IList<AlbumDto> albumDtos);

        AlbumDto ConvertToDto(Album album);

        Album ConvertToEntity(AlbumDto albumDto);
    }
}