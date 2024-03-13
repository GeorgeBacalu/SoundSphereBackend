using Microsoft.EntityFrameworkCore;
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
            User existingUser = _context.Users.Find(playlist.User.Id);
            if (existingUser != null)
            {
                _context.Entry(existingUser).State = EntityState.Unchanged;
                playlist.User = existingUser;
            }

            _context.Playlists.Add(playlist);
            _context.SaveChanges();
            return playlist;
        }

        public Playlist UpdateById(Playlist playlist, Guid id)
        {
            Playlist playlistToUpdate = FindById(id);
            DateTime CreatedAt = playlistToUpdate.CreatedAt;
            _context.Entry(playlistToUpdate).CurrentValues.SetValues(playlist);
            playlistToUpdate.CreatedAt = CreatedAt;
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