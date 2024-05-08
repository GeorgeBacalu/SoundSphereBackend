using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface ISongService
    {
        IList<SongDto> FindAll();

        IList<SongDto> FindAllActive();

        IList<SongDto> FindAllPagination(SongPaginationRequest payload);

        IList<SongDto> FindAllActivePagination(SongPaginationRequest payload);

        SongDto FindById(Guid id);

        SongDto Save(SongDto songDto);

        SongDto UpdateById(SongDto songDto, Guid id);

        SongDto DisableById(Guid id);
    }
}