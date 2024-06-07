using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request.Pagination;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface ISongService
    {
        IList<SongDto> GetAll(SongPaginationRequest payload);

        SongDto GetById(Guid id);

        SongDto Add(SongDto songDto);

        SongDto UpdateById(SongDto songDto, Guid id);

        SongDto DeleteById(Guid id);
    }
}