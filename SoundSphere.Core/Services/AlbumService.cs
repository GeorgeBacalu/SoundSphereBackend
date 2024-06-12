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

        public IList<AlbumDto> GetAll(AlbumPaginationRequest payload)
        {
            IList<AlbumDto> albumsDtos = _albumRepository.GetAll(payload).ToDtos(_mapper);
            return albumsDtos;
        }

        public AlbumDto GetById(Guid id)
        {
            AlbumDto albumDto = _albumRepository.GetById(id).ToDto(_mapper); ;
            return albumDto;
        }

        public AlbumDto Add(AlbumDto albumDto)
        {
            Album albumToCreate = albumDto.ToEntity(_mapper);
            _albumRepository.AddAlbumLink(albumToCreate);
            AlbumDto createdAlbumDto = _albumRepository.Add(albumToCreate).ToDto(_mapper);
            return createdAlbumDto;
        }

        public AlbumDto UpdateById(AlbumDto albumDto, Guid id)
        {
            Album albumToUpdate = albumDto.ToEntity(_mapper);
            AlbumDto updatedAlbumDto = _albumRepository.UpdateById(albumToUpdate, id).ToDto(_mapper);
            return updatedAlbumDto;
        }

        public AlbumDto DeleteById(Guid id)
        {
            AlbumDto deletedAlbumDto = _albumRepository.DeleteById(id).ToDto(_mapper);
            return deletedAlbumDto;
        }
    }
}