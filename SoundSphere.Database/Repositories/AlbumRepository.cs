using Microsoft.EntityFrameworkCore;
using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;

namespace SoundSphere.Database.Repositories
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly SoundSphereContext _context;

        public AlbumRepository(SoundSphereContext context) => _context = context;

        public IList<Album> FindAll() => _context.Albums
            .Include(album => album.SimilarAlbums)
            .ToList();

        public IList<Album> FindAllActive() => _context.Albums
            .Where(album => album.IsActive)
            .Include(album => album.SimilarAlbums)
            .ToList();

        public Album FindById(Guid id) => _context.Albums
            .Include(album => album.SimilarAlbums)
            .FirstOrDefault(album => album.Id == id)
            ?? throw new ResourceNotFoundException($"Album with id {id} not found!");

        public Album Save(Album album)
        {
            _context.Albums.Add(album);
            _context.SaveChanges();
            return album;
        }

        public Album UpdateById(Album album, Guid id)
        {
            Album albumToUpdate = FindById(id);
            albumToUpdate.Title = album.Title;
            albumToUpdate.ImageUrl = album.ImageUrl;
            albumToUpdate.ReleaseDate = album.ReleaseDate;
            albumToUpdate.SimilarAlbums = album.SimilarAlbums;
            _context.SaveChanges();
            return albumToUpdate;
        }

        public Album DisableById(Guid id)
        {
            Album albumToDisable = FindById(id);
            albumToDisable.IsActive = false;
            _context.SaveChanges();
            return albumToDisable;
        }

        public void AddAlbumLink(Album album) =>
            album.SimilarAlbums = album.SimilarAlbums
                            .Select(similarAlbum => _context.Albums.Find(similarAlbum.SimilarAlbumId))
                            .Where(similarAlbum => similarAlbum != null)
                            .Select(similarAlbum => new AlbumLink
                            {
                                Album = album,
                                SimilarAlbum = similarAlbum
                            })
                            .ToList();
    }
}