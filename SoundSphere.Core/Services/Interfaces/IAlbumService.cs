using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request.Pagination;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IAlbumService
    {
        IList<AlbumDto> GetAll(AlbumPaginationRequest? payload);

        AlbumDto GetById(Guid id);

        AlbumDto Add(AlbumDto albumDto);

        AlbumDto UpdateById(AlbumDto albumDto, Guid id);

        AlbumDto DeleteById(Guid id);

        IList<AlbumDto> GetRecommendations(int nrRecommendations);
    }
}