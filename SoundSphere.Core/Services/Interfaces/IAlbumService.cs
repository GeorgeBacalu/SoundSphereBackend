using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IAlbumService
    {
        IList<AlbumDto> FindAll();

        IList<AlbumDto> FindAllActive();

        IList<AlbumDto> FindAllPagination(AlbumPaginationRequest payload);

        IList<AlbumDto> FindAllActivePagination(AlbumPaginationRequest payload);

        AlbumDto FindById(Guid id);

        AlbumDto Save(AlbumDto albumDto);

        AlbumDto UpdateById(AlbumDto albumDto, Guid id);

        AlbumDto DisableById(Guid id);
    }
}