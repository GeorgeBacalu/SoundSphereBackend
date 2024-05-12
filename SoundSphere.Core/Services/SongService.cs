using AutoMapper;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Services
{
    public class SongService : ISongService
    {
        private readonly ISongRepository _songRepository;
        private readonly IAlbumRepository _albumRepository;
        private readonly IArtistRepository _artistRepository;
        private readonly IMapper _mapper;

        public SongService(ISongRepository songRepository, IAlbumRepository albumRepository, IArtistRepository artistRepository, IMapper mapper) => 
            (_songRepository, _albumRepository, _artistRepository, _mapper) = (songRepository, albumRepository, artistRepository, mapper);

        public IList<SongDto> FindAll() => _songRepository.FindAll().ToDtos(_mapper);

        public IList<SongDto> FindAllActive() => _songRepository.FindAllActive().ToDtos(_mapper);

        public IList<SongDto> FindAllPagination(SongPaginationRequest payload) => _songRepository.FindAllPagination(payload).ToDtos(_mapper);

        public IList<SongDto> FindAllActivePagination(SongPaginationRequest payload) => _songRepository.FindAllActivePagination(payload).ToDtos(_mapper);

        public SongDto FindById(Guid id) => _songRepository.FindById(id).ToDto(_mapper);

        public SongDto Save(SongDto songDto)
        {
            Song song = songDto.ToEntity(_albumRepository, _artistRepository, _mapper);
            if (song.Id == Guid.Empty) song.Id = Guid.NewGuid();
            song.IsActive = true;
            _songRepository.LinkSongToAlbum(song);
            _songRepository.LinkSongToArtists(song);
            _songRepository.AddSongLink(song);
            _songRepository.AddUserSong(song);
            return _songRepository.Save(song).ToDto(_mapper);
        }

        public SongDto UpdateById(SongDto songDto, Guid id) => _songRepository.UpdateById(songDto.ToEntity(_albumRepository, _artistRepository, _mapper), id).ToDto(_mapper);

        public SongDto DisableById(Guid id) => _songRepository.DisableById(id).ToDto(_mapper);
    }
}