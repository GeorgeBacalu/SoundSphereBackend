using AutoMapper;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Services
{
    public class ArtistService : IArtistService
    {
        private readonly IArtistRepository _artistRepository;
        private readonly IMapper _mapper;

        public ArtistService(IArtistRepository artistRepository, IMapper mapper) => (_artistRepository, _mapper) = (artistRepository, mapper);

        public IList<ArtistDto> FindAll() => ConvertToDtos(_artistRepository.FindAll());

        public IList<ArtistDto> FindAllActive() => ConvertToDtos(_artistRepository.FindAllActive());

        public ArtistDto FindById(Guid id) => ConvertToDto(_artistRepository.FindById(id));

        public ArtistDto Save(ArtistDto artistDto)
        {
            Artist artist = ConvertToEntity(artistDto);
            if (artist.Id == Guid.Empty) artist.Id = Guid.NewGuid();
            artist.IsActive = true;
            _artistRepository.AddArtistLink(artist);
            _artistRepository.AddUserArtist(artist);
            return ConvertToDto(_artistRepository.Save(artist));
        }

        public ArtistDto UpdateById(ArtistDto artistDto, Guid id) => ConvertToDto(_artistRepository.UpdateById(ConvertToEntity(artistDto), id));

        public ArtistDto DisableById(Guid id) => ConvertToDto(_artistRepository.DisableById(id));

        public IList<ArtistDto> ConvertToDtos(IList<Artist> artists) => artists.Select(ConvertToDto).ToList();

        public IList<Artist> ConvertToEntities(IList<ArtistDto> artistDtos) => artistDtos.Select(ConvertToEntity).ToList();

        public ArtistDto ConvertToDto(Artist artist)
        {
            ArtistDto artistDto = _mapper.Map<ArtistDto>(artist);
            artistDto.SimilarArtistsIds = artist.SimilarArtists
                .Select(artistLink => artistLink.SimilarArtistId)
                .ToList();
            return artistDto;
        }

        public Artist ConvertToEntity(ArtistDto artistDto)
        {
            Artist artist = _mapper.Map<Artist>(artistDto);
            artist.SimilarArtists = artistDto.SimilarArtistsIds
                .Select(id => new ArtistLink { ArtistId = artistDto.Id, SimilarArtistId = id })
                .ToList();
            return artist;
        }
    }
}