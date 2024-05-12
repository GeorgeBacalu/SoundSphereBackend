using AutoMapper;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly IMapper _mapper;

        public AlbumService(IAlbumRepository albumRepository, IMapper mapper) => (_albumRepository, _mapper) = (albumRepository, mapper);

        public IList<AlbumDto> FindAll() => _albumRepository.FindAll().ToDtos(_mapper);

        public IList<AlbumDto> FindAllActive() => _albumRepository.FindAllActive().ToDtos(_mapper);

        public IList<AlbumDto> FindAllPagination(AlbumPaginationRequest payload) => _albumRepository.FindAllPagination(payload).ToDtos(_mapper);

        public IList<AlbumDto> FindAllActivePagination(AlbumPaginationRequest payload) => _albumRepository.FindAllActivePagination(payload).ToDtos(_mapper);

        public AlbumDto FindById(Guid id) => _albumRepository.FindById(id).ToDto(_mapper);

        public AlbumDto Save(AlbumDto albumDto)
        {
            Album album = albumDto.ToEntity(_mapper);
            if (album.Id == Guid.Empty) album.Id = Guid.NewGuid();
            album.IsActive = true;
            _albumRepository.AddAlbumLink(album);
            return _albumRepository.Save(album).ToDto(_mapper);
        }

        public AlbumDto UpdateById(AlbumDto albumDto, Guid id) => _albumRepository.UpdateById(albumDto.ToEntity(_mapper), id).ToDto(_mapper);

        public AlbumDto DisableById(Guid id) => _albumRepository.DisableById(id).ToDto(_mapper);
    }
}