using Microsoft.EntityFrameworkCore;
using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;

namespace SoundSphere.Database.Repositories
{
    public class SongRepository : ISongRepository
    {
        private readonly SoundSphereContext _context;

        public SongRepository(SoundSphereContext context) => _context = context;

        public IList<Song> FindAll() => _context.Songs
            .Include(song => song.Album)
            .Include(song => song.Artists)
            .Include(song => song.SimilarSongs)
            .ToList();

        public IList<Song> FindAllActive() => _context.Songs
            .Where(song => song.IsActive)
            .Include(song => song.Album)
            .Include(song => song.Artists)
            .Include(song => song.SimilarSongs)
            .ToList();

        public Song FindById(Guid id) => _context.Songs
            .Include(song => song.Album)
            .Include(song => song.Artists)
            .Include(song => song.SimilarSongs)
            .FirstOrDefault(song => song.Id == id)
            ?? throw new ResourceNotFoundException($"Song with id {id} not found!");

        public Song Save(Song song)
        {
            _context.Songs.Add(song);
            _context.SaveChanges();
            return song;
        }

        public Song UpdateById(Song song, Guid id)
        {
            Song songToUpdate = FindById(id);
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

        public Song DisableById(Guid id)
        {
            Song songToDisable = FindById(id);
            songToDisable.IsActive = false;
            _context.SaveChanges();
            return songToDisable;
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

        public void LinkSongToArtists(Song song)
        {
            song.Artists = song.Artists
                            .Select(artist => _context.Artists.Find(artist.Id))
                            .Where(artist => artist != null)
                            .ToList();
            song.Artists.ToList().ForEach(artist => _context.Entry(artist).State = EntityState.Unchanged);
        }

        public void AddSongLink(Song song) =>
            song.SimilarSongs = song.SimilarSongs
                            .Select(similarSong => _context.Songs.Find(similarSong.SimilarSongId))
                            .Where(similarSong => similarSong != null)
                            .Select(similarSong => new SongLink
                            {
                                Song = song,
                                SimilarSong = similarSong
                            })
                            .ToList();

        public void AddUserSong(Song song) =>
            _context.AddRange(_context.Users
                            .Select(user => new UserSong
                            {
                                User = user,
                                Song = song,
                                PlayCount = 0
                            })
                            .ToList());
    }
}