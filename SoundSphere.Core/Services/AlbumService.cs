using AutoMapper;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly IMapper _mapper;

        public AlbumService(IAlbumRepository albumRepository, IMapper mapper) => (_albumRepository, _mapper) = (albumRepository, mapper);

        public IList<AlbumDto> GetAll(AlbumPaginationRequest payload) => _albumRepository.GetAll(payload).ToDtos(_mapper);

        public AlbumDto GetById(Guid id) => _albumRepository.GetById(id).ToDto(_mapper);

        public AlbumDto Add(AlbumDto albumDto)
        {
            Album album = albumDto.ToEntity(_mapper);
            _albumRepository.AddAlbumLink(album);
            return _albumRepository.Add(album).ToDto(_mapper);
        }

        public AlbumDto UpdateById(AlbumDto albumDto, Guid id) => _albumRepository.UpdateById(albumDto.ToEntity(_mapper), id).ToDto(_mapper);

        public AlbumDto DeleteById(Guid id) => _albumRepository.DeleteById(id).ToDto(_mapper);
    }
}