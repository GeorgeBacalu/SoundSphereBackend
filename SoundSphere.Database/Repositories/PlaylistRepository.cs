using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Database.Repositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly SoundSphereContext _context;
        
        public PlaylistRepository(SoundSphereContext context) => _context = context;

        public IList<Playlist> FindAll() => _context.Playlists.ToList();

        public Playlist FindById(Guid id) => _context.Playlists.Find(id) ?? throw new Exception($"Playlist with id {id} not found!");

        public Playlist Save(Playlist playlist)
        {
            if (playlist == null) throw new Exception("Can't persist null playlist to DB!");
            if (playlist.Id == Guid.Empty) playlist.Id = Guid.NewGuid();
            playlist.IsActive = true;
            playlist.CreatedAt = DateTime.Now;
            _context.Playlists.Add(playlist);
            _context.SaveChanges();
            return playlist;
        }

        public Playlist UpdateById(Playlist playlist, Guid id)
        {
            if (playlist == null) throw new Exception("Can't persist null playlist to DB!");
            Playlist playlistToUpdate = FindById(id);
            _context.Entry(playlistToUpdate).CurrentValues.SetValues(playlist);
            _context.SaveChanges();
            return playlistToUpdate;
        }

        public Playlist DisableById(Guid id)
        {
            Playlist playlistToDisable = FindById(id);
            playlistToDisable.IsActive = false;
            _context.SaveChanges();
            return playlistToDisable;
        }
    }
}