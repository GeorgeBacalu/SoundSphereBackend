using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface ISongService
    {
        IList<SongDto> GetAll();

        IList<SongDto> GetAllActive();

        IList<SongDto> GetAllPagination(SongPaginationRequest payload);

        IList<SongDto> GetAllActivePagination(SongPaginationRequest payload);

        SongDto GetById(Guid id);

        SongDto Add(SongDto songDto);

        SongDto UpdateById(SongDto songDto, Guid id);

        SongDto DeleteById(Guid id);
    }
}