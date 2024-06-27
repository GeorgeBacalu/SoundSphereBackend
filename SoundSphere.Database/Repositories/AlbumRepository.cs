using Microsoft.EntityFrameworkCore;
using SoundSphere.Database.Context;
using SoundSphere.Database.Dtos.Request.Pagination;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Extensions;
using SoundSphere.Database.Repositories.Interfaces;
using SoundSphere.Infrastructure.Exceptions;
using static SoundSphere.Database.Constants;

namespace SoundSphere.Database.Repositories
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly SoundSphereDbContext _context;

        public AlbumRepository(SoundSphereDbContext context) => _context = context;

        public IList<Album> GetAll(AlbumPaginationRequest? payload)
        {
            IList<Album> albums = _context.Albums
                .Include(album => album.SimilarAlbums)
                .Where(album => album.DeletedAt == null)
                .ApplyPagination(payload)
                .ToList();
            return albums;
        }

        public Album GetById(Guid id)
        {
            Album? album = _context.Albums
                .Include(album => album.SimilarAlbums)
                .Where(album => album.DeletedAt == null)
                .FirstOrDefault(album => album.Id.Equals(id));
            if (album == null)
                throw new ResourceNotFoundException(string.Format(AlbumNotFound, id));
            return album;
        }

        public Album Add(Album album)
        {
            if (album.Id == Guid.Empty)
                album.Id = Guid.NewGuid();
            album.CreatedAt = DateTime.Now;
            _context.Albums.Add(album);
            _context.SaveChanges();
            return album;
        }

        public Album UpdateById(Album album, Guid id)
        {
            Album albumToUpdate = GetById(id);
            albumToUpdate.Title = album.Title;
            albumToUpdate.ImageUrl = album.ImageUrl;
            albumToUpdate.ReleaseDate = album.ReleaseDate;
            albumToUpdate.SimilarAlbums = album.SimilarAlbums;
            if (_context.Entry(albumToUpdate).State == EntityState.Modified)
                albumToUpdate.UpdatedAt = DateTime.Now;
            _context.SaveChanges();
            return albumToUpdate;
        }

        public Album DeleteById(Guid id)
        {
            Album albumToDelete = GetById(id);
            albumToDelete.DeletedAt = DateTime.Now;
            _context.SaveChanges();
            return albumToDelete;
        }

        public void AddAlbumLink(Album album) => album.SimilarAlbums = album.SimilarAlbums
            .Select(similarAlbum => _context.Albums.Find(similarAlbum.SimilarAlbumId))
            .Where(similarAlbum => similarAlbum != null)
            .Select(similarAlbum => new AlbumLink { Album = album, SimilarAlbum = similarAlbum })
            .ToList();
    }
}