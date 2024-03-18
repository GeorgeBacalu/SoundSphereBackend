using SoundSphere.Database.Dtos;
using SoundSphere.Database.Entities;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface ISongService
    {
        IList<SongDto> FindAll();

        SongDto FindById(Guid id);

        SongDto Save(SongDto songDto);

        SongDto UpdateById(SongDto songDto, Guid id);

        SongDto DisableById(Guid id);

        IList<SongDto> ConvertToDtos(IList<Song> songs);

        IList<Song> ConvertToEntities(IList<SongDto> songDtos);

        SongDto ConvertToDto(Song song);

        Song ConvertToEntity(SongDto songDto);
    }
}