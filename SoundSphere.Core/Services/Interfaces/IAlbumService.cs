using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IAlbumService
    {
        IList<AlbumDto> GetAll();

        IList<AlbumDto> GetAllActive();

        IList<AlbumDto> GetAllPagination(AlbumPaginationRequest payload);

        IList<AlbumDto> GetAllActivePagination(AlbumPaginationRequest payload);

        AlbumDto GetById(Guid id);

        AlbumDto Add(AlbumDto albumDto);

        AlbumDto UpdateById(AlbumDto albumDto, Guid id);

        AlbumDto DeleteById(Guid id);
    }
}