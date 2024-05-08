using Microsoft.EntityFrameworkCore;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Request;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Extensions;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;

namespace SoundSphere.Database.Repositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly SoundSphereDbContext _context;

        public PlaylistRepository(SoundSphereDbContext context) => _context = context;

        public IList<Playlist> FindAll() => _context.Playlists
            .Include(playlist => playlist.User)
            .ToList();

        public IList<Playlist> FindAllActive() => _context.Playlists
            .Include(playlist => playlist.User)
            .Where(playlist => playlist.IsActive)
            .ToList();

        public IList<Playlist> FindAllPagination(PlaylistPaginationRequest payload) => _context.Playlists
            .Include(playlist => playlist.User)
            .Filter(payload)
            .Sort(payload)
            .Paginate(payload)
            .ToList();

        public IList<Playlist> FindAllActivePagination(PlaylistPaginationRequest payload) => _context.Playlists
            .Include(playlist => playlist.User)
            .Where(playlist => playlist.IsActive)
            .Filter(payload)
            .Sort(payload)
            .Paginate(payload)
            .ToList();

        public Playlist FindById(Guid id) => _context.Playlists
            .Include(playlist => playlist.User)
            .Where(playlist => playlist.IsActive)
            .FirstOrDefault(playlist => playlist.Id == id)
            ?? throw new ResourceNotFoundException(string.Format(Constants.PlaylistNotFound, id));

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