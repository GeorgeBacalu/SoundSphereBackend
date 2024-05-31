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
    public class SongRepository : ISongRepository
    {
        private readonly SoundSphereDbContext _context;

        public SongRepository(SoundSphereDbContext context) => _context = context;

        public IList<Song> GetAll() => _context.Songs
            .Include(song => song.Album)
            .Include(song => song.Artists)
            .Include(song => song.SimilarSongs)
            .ToList();

        public IList<Song> GetAllActive() => _context.Songs
            .Include(song => song.Album)
            .Include(song => song.Artists)
            .Include(song => song.SimilarSongs)
            .Where(song => song.IsActive)
            .ToList();

        public IList<Song> GetAllPagination(SongPaginationRequest payload) => _context.Songs
            .Include(song => song.Album)
            .Include(song => song.Artists)
            .Include(song => song.SimilarSongs)
            .Filter(payload)
            .Sort(payload)
            .Paginate(payload)
            .ToList();

        public IList<Song> GetAllActivePagination(SongPaginationRequest payload) => _context.Songs
            .Include(song => song.Album)
            .Include(song => song.Artists)
            .Include(song => song.SimilarSongs)
            .Where(song => song.IsActive)
            .Filter(payload)
            .Sort(payload)
            .Paginate(payload)
            .ToList();

        public Song GetById(Guid id) => _context.Songs
            .Include(song => song.Album)
            .Include(song => song.Artists)
            .Include(song => song.SimilarSongs)
            .Where(song => song.IsActive)
            .FirstOrDefault(song => song.Id == id)
            ?? throw new ResourceNotFoundException(string.Format(SongNotFound, id));

        public Song Add(Song song)
        {
            if (song.Id == Guid.Empty) song.Id = Guid.NewGuid();
            song.IsActive = true;
            _context.Songs.Add(song);
            _context.SaveChanges();
            return song;
        }

        public Song UpdateById(Song song, Guid id)
        {
            Song songToUpdate = GetById(id);
            songToUpdate.Title = song.Title;
            songToUpdate.ImageUrl = song.ImageUrl;
            songToUpdate.Genre = song.Genre;
            songToUpdate.ReleaseDate = song.ReleaseDate;
            songToUpdate.DurationSeconds = song.DurationSeconds;
            songToUpdate.Album = song.Album;
            songToUpdate.Artists = song.Artists;
            songToUpdate.SimilarSongs = song.SimilarSongs;
            _context.SaveChanges();
            return songToUpdate;
        }

        public Song DeleteById(Guid id)
        {
            Song songToDelete = GetById(id);
            songToDelete.IsActive = false;
            _context.SaveChanges();
            return songToDelete;
        }

        public void LinkSongToAlbum(Song song)
        {
            Album existingAlbum = _context.Albums.Find(song.Album.Id);
            if (existingAlbum != null)
            {
                _context.Entry(existingAlbum).State = EntityState.Unchanged;
                song.Album = existingAlbum;
            }
        }

        public void LinkSongToArtists(Song song) => song.Artists = song.Artists
            .Select(artist => _context.Artists.Find(artist.Id))
            .Where(artist => artist != null)
            .Select(artist => { _context.Entry(artist).State = EntityState.Unchanged; return artist; })
            .ToList();

        public void AddSongLink(Song song) => song.SimilarSongs = song.SimilarSongs
            .Select(similarSong => _context.Songs.Find(similarSong.SimilarSongId))
            .Where(similarSong => similarSong != null)
            .Select(similarSong => new SongLink { Song = song, SimilarSong = similarSong })
            .ToList();

        public void AddUserSong(Song song) => _context.AddRange(_context.Users
            .Select(user => new UserSong { User = user, Song = song, PlayCount = 0 })
            .ToList());
    }
}