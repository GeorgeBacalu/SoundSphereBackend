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
    public class AlbumRepository : IAlbumRepository
    {
        private readonly SoundSphereDbContext _context;

        public AlbumRepository(SoundSphereDbContext context) => _context = context;

        public IList<Album> GetAll() => _context.Albums
            .Include(album => album.SimilarAlbums)
            .ToList();

        public IList<Album> GetAllActive() => _context.Albums
            .Include(album => album.SimilarAlbums)
            .Where(album => album.IsActive)
            .ToList();

        public IList<Album> GetAllPagination(AlbumPaginationRequest payload) => _context.Albums
            .Include(album => album.SimilarAlbums)
            .Filter(payload)
            .Sort(payload)
            .Paginate(payload)
            .ToList();

        public IList<Album> GetAllActivePagination(AlbumPaginationRequest payload) => _context.Albums
            .Include(album => album.SimilarAlbums)
            .Where(album => album.IsActive)
            .Filter(payload)
            .Sort(payload)
            .Paginate(payload)
            .ToList();

        public Album GetById(Guid id) => _context.Albums
            .Include(album => album.SimilarAlbums)
            .Where(album => album.IsActive)
            .FirstOrDefault(album => album.Id == id)
            ?? throw new ResourceNotFoundException(string.Format(AlbumNotFound, id));

        public Album Add(Album album)
        {
            if (album.Id == Guid.Empty) album.Id = Guid.NewGuid();
            album.IsActive = true;
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
            _context.SaveChanges();
            return albumToUpdate;
        }

        public Album DeleteById(Guid id)
        {
            Album albumToDelete = GetById(id);
            albumToDelete.IsActive = false;
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