using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Database.Repositories
{
    public class ArtistService : IArtistService
    {
        private readonly IArtistRepository _artistRepository;

        public ArtistService(IArtistRepository artistRepository) => _artistRepository = artistRepository;

        public IList<Artist> FindAll() => _artistRepository.FindAll();

        public Artist FindById(Guid id) => _artistRepository.FindById(id);

        public Artist Save(Artist artist)
        {
            if (artist == null) throw new Exception("Can't persist null artist to DB!");
            if (artist.Id == Guid.Empty) artist.Id = Guid.NewGuid();
            artist.IsActive = true;

            _artistRepository.AddArtistLink(artist);
            _artistRepository.LinkArtistToUser(artist);

            return _artistRepository.Save(artist);
        }

        public Artist UpdateById(Artist artist, Guid id)
        {
            if (artist == null) throw new Exception("Can't persist null artist to DB!");
            return _artistRepository.UpdateById(artist, id);
        }

        public Artist DisableById(Guid id) => _artistRepository.DisableById(id);
    }
}