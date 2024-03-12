using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Database.Repositories
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly SoundSphereContext _context;

        public ArtistRepository(SoundSphereContext context) => _context = context;

        public IList<Artist> FindAll() => _context.Artists.ToList();

        public Artist FindById(Guid id) => _context.Artists.Find(id) ?? throw new Exception($"Artist with id {id} not found!");

        public Artist Save(Artist artist)
        {
            if (artist == null) throw new Exception("Can't persist null artist to DB!");
            if (artist.Id == Guid.Empty) artist.Id = Guid.NewGuid();
            artist.IsActive = true;
            _context.Artists.Add(artist);
            _context.SaveChanges();
            return artist;
        }

        public Artist UpdateById(Artist artist, Guid id)
        {
            if (artist == null) throw new Exception("Can't persist null artist to DB!");
            Artist artistToUpdate = FindById(id);
            _context.Entry(artistToUpdate).CurrentValues.SetValues(artist);
            _context.SaveChanges();
            return artistToUpdate;
        }

        public Artist DisableById(Guid id)
        {
            Artist artistToDisable = FindById(id);
            artistToDisable.IsActive = false;
            _context.SaveChanges();
            return artistToDisable;
        }
    }
}