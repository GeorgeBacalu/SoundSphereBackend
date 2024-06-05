using Microsoft.EntityFrameworkCore;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Extensions;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;
using static SoundSphere.Database.Constants;

namespace SoundSphere.Database.Repositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly SoundSphereDbContext _context;

        public PlaylistRepository(SoundSphereDbContext context) => _context = context;

        public IList<Playlist> GetAll(PlaylistPaginationRequest payload) => _context.Playlists
            .Include(playlist => playlist.User)
            .Where(playlist => playlist.DeletedAt == null)
            .Filter(payload)
            .Sort(payload)
            .Paginate(payload)
            .ToList();

        public Playlist GetById(Guid id) => _context.Playlists
            .Include(playlist => playlist.User)
            .Where(playlist => playlist.DeletedAt == null)
            .FirstOrDefault(playlist => playlist.Id == id)
            ?? throw new ResourceNotFoundException(string.Format(PlaylistNotFound, id));

        public Playlist Add(Playlist playlist)
        {
            if (playlist.Id == Guid.Empty) playlist.Id = Guid.NewGuid();
            playlist.CreatedAt = DateTime.Now;
            _context.Playlists.Add(playlist);
            _context.SaveChanges();
            return playlist;
        }

        public Playlist UpdateById(Playlist playlist, Guid id)
        {
            Playlist playlistToUpdate = GetById(id);
            playlistToUpdate.Title = playlist.Title;
            if (_context.Entry(playlistToUpdate).State == EntityState.Modified)
                playlistToUpdate.UpdatedAt = DateTime.Now;
            _context.SaveChanges();
            return playlistToUpdate;
        }

        public Playlist DeleteById(Guid id)
        {
            Playlist playlistToDelete = GetById(id);
            playlistToDelete.DeletedAt = DateTime.Now;
            _context.SaveChanges();
            return playlistToDelete;
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