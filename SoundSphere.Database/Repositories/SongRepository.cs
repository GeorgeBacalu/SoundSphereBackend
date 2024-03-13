using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Database.Repositories
{
    public class SongRepository : ISongRepository
    {
        private readonly SoundSphereContext _context; 
        
        public SongRepository(SoundSphereContext context) => _context = context;

        public IList<Song> FindAll() => _context.Songs.ToList();

        public Song FindById(Guid id) => _context.Songs.Find(id) ?? throw new Exception($"Song with id {id} not found!");

        public Song Save(Song song)
        {
            _context.Songs.Add(song);
            _context.SaveChanges();
            return song;
        }

        public Song UpdateById(Song song, Guid id)
        {
            Song songToUpdate = FindById(id);
            _context.Entry(songToUpdate).CurrentValues.SetValues(song);
            _context.SaveChanges();
            return songToUpdate;
        }

        public Song DisableById(Guid id)
        {
            Song songToDisable = FindById(id);
            songToDisable.IsActive = false;
            _context.SaveChanges();
            return songToDisable;
        }
    }
}