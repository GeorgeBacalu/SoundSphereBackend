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

        public IList<ArtistDto> FindAll() => _artistRepository.FindAll().ToDtos(_mapper);

        public IList<ArtistDto> FindAllActive() => _artistRepository.FindAllActive().ToDtos(_mapper);

        public IList<ArtistDto> FindAllPagination(ArtistPaginationRequest payload) => _artistRepository.FindAllPagination(payload).ToDtos(_mapper);

        public IList<ArtistDto> FindAllActivePagination(ArtistPaginationRequest payload) => _artistRepository.FindAllActivePagination(payload).ToDtos(_mapper);

        public ArtistDto FindById(Guid id) => _artistRepository.FindById(id).ToDto(_mapper);

        public ArtistDto Save(ArtistDto artistDto)
        {
            Artist artist = artistDto.ToEntity(_mapper);
            if (artist.Id == Guid.Empty) artist.Id = Guid.NewGuid();
            artist.IsActive = true;
            _artistRepository.AddArtistLink(artist);
            _artistRepository.AddUserArtist(artist);
            return _artistRepository.Save(artist).ToDto(_mapper);
        }

        public ArtistDto UpdateById(ArtistDto artistDto, Guid id) => _artistRepository.UpdateById(artistDto.ToEntity(_mapper), id).ToDto(_mapper);

        public ArtistDto DisableById(Guid id) => _artistRepository.DisableById(id).ToDto(_mapper);
    }
}