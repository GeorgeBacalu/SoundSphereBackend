using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Database.Repositories
{
    public class ArtistService : IArtistService
    {
        private readonly IArtistRepository _artistRepository;

        public ArtistService(IArtistRepository artistRepository) => _artistRepository = artistRepository;

        public IList<ArtistDto> FindAll() => ConvertToDtos(_artistRepository.FindAll());

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

        public ArtistDto ConvertToDto(Artist artist) => new ArtistDto
        {
            Id = artist.Id,
            Name = artist.Name,
            ImageUrl = artist.ImageUrl,
            Bio = artist.Bio,
            SimilarArtistsIds = artist.SimilarArtists
                .Select(artist => artist.SimilarArtistId)
                .ToList(),
            IsActive = artist.IsActive
        };

        public Artist ConvertToEntity(ArtistDto artistDto) => new Artist
        {
            Id = artistDto.Id,
            Name = artistDto.Name,
            ImageUrl = artistDto.ImageUrl,
            Bio = artistDto.Bio,
            SimilarArtists = artistDto.SimilarArtistsIds
                .Select(id => new ArtistLink
                {
                    ArtistId = artistDto.Id,
                    SimilarArtistId = id
                })
                .ToList(),
            IsActive = artistDto.IsActive
        };
    }
}