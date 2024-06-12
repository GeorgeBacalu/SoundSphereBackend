using AutoMapper;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Services
{
    public class ArtistService : IArtistService
    {
        private readonly IArtistRepository _artistRepository;
        private readonly IMapper _mapper;

        public ArtistService(IArtistRepository artistRepository, IMapper mapper) => (_artistRepository, _mapper) = (artistRepository, mapper);

        public IList<ArtistDto> GetAll(ArtistPaginationRequest payload)
        {
            IList<ArtistDto> artistDtos = _artistRepository.GetAll(payload).ToDtos(_mapper);
            return artistDtos;
        }

        public ArtistDto GetById(Guid id)
        {
            ArtistDto artistDto = _artistRepository.GetById(id).ToDto(_mapper);
            return artistDto;
        }

        public ArtistDto Add(ArtistDto artistDto)
        {
            Artist artistToCreate = artistDto.ToEntity(_mapper);
            _artistRepository.AddArtistLink(artistToCreate);
            _artistRepository.AddUserArtist(artistToCreate);
            ArtistDto createdArtistDto = _artistRepository.Add(artistToCreate).ToDto(_mapper);
            return createdArtistDto;
        }

        public ArtistDto UpdateById(ArtistDto artistDto, Guid id)
        {
            Artist artistToUpdate = artistDto.ToEntity(_mapper);
            ArtistDto updatedArtistDto = _artistRepository.UpdateById(artistToUpdate, id).ToDto(_mapper);
            return updatedArtistDto;
        }

        public ArtistDto DeleteById(Guid id)
        {
            ArtistDto deletedArtistDto = _artistRepository.DeleteById(id).ToDto(_mapper);
            return deletedArtistDto;
        }
    }
}