using Microsoft.EntityFrameworkCore;
using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;

namespace SoundSphere.Database.Repositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly SoundSphereContext _context;

        public PlaylistRepository(SoundSphereContext context) => _context = context;

        public IList<Playlist> FindAll() => _context.Playlists
            .Include(playlist => playlist.User)
            .ToList();

        public Playlist FindById(Guid id) => _context.Playlists
            .Include(playlist => playlist.User)
            .FirstOrDefault(playlist => playlist.Id == id)
            ?? throw new ResourceNotFoundException($"Playlist with id {id} not found!");

        public Playlist Save(Playlist playlist)
        {
            _context.Playlists.Add(playlist);
            _context.SaveChanges();
            return playlist;
        }

        public Playlist UpdateById(Playlist playlist, Guid id)
        {
            Playlist playlistToUpdate = FindById(id);
            playlistToUpdate.Title = playlist.Title;
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

        public void LinkPlaylistToUser(Playlist playlist)
        {
            User existingUser = _context.Users.Find(playlist.User.Id);
            if (existingUser != null)
            {
                _context.Entry(existingUser).State = EntityState.Unchanged;
                playlist.User = existingUser;
            }
        }
    }
}