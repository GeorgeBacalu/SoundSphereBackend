using AutoMapper;
using SoundSphere.Core.Mappings;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Services
{
    public class ArtistService : IArtistService
    {
        private readonly IArtistRepository _artistRepository;
        private readonly IMapper _mapper;

        public ArtistService(IArtistRepository artistRepository, IMapper mapper) => (_artistRepository, _mapper) = (artistRepository, mapper);

        public IList<ArtistDto> GetAll(ArtistPaginationRequest payload) => _artistRepository.GetAll(payload).ToDtos(_mapper);

        public ArtistDto GetById(Guid id) => _artistRepository.GetById(id).ToDto(_mapper);

        public ArtistDto Add(ArtistDto artistDto)
        {
            Artist artist = artistDto.ToEntity(_mapper);
            _artistRepository.AddArtistLink(artist);
            _artistRepository.AddUserArtist(artist);
            return _artistRepository.Add(artist).ToDto(_mapper);
        }

        public ArtistDto UpdateById(ArtistDto artistDto, Guid id) => _artistRepository.UpdateById(artistDto.ToEntity(_mapper), id).ToDto(_mapper);

        public ArtistDto DeleteById(Guid id) => _artistRepository.DeleteById(id).ToDto(_mapper);
    }
}