using AutoMapper;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request.Pagination;
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

        public IList<SongDto> GetAll(SongPaginationRequest payload)
        {
            IList<SongDto> songDtos = _songRepository.GetAll(payload).ToDtos(_mapper);
            return songDtos;
        }

        public SongDto GetById(Guid id)
        {
            SongDto songDto = _songRepository.GetById(id).ToDto(_mapper);
            return songDto;
        }

        public SongDto Add(SongDto songDto)
        {
            Song songToCreate = songDto.ToEntity(_albumRepository, _artistRepository, _mapper);
            _songRepository.LinkSongToAlbum(songToCreate);
            _songRepository.LinkSongToArtists(songToCreate);
            _songRepository.AddSongLink(songToCreate);
            _songRepository.AddUserSong(songToCreate);
            SongDto createdSongDto = _songRepository.Add(songToCreate).ToDto(_mapper);
            return createdSongDto;
        }

        public SongDto UpdateById(SongDto songDto, Guid id)
        {
            Song songToUpdate = songDto.ToEntity(_albumRepository, _artistRepository, _mapper);
            SongDto updatedSongDto = _songRepository.UpdateById(songToUpdate, id).ToDto(_mapper);
            return updatedSongDto;
        }

        public SongDto DeleteById(Guid id)
        {
            SongDto deletedSongDto = _songRepository.DeleteById(id).ToDto(_mapper);
            return deletedSongDto;
        }
    }
}