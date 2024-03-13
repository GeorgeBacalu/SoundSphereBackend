using SoundSphere.Database.Context;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Database.Repositories
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly SoundSphereContext _context;

        public AlbumRepository(SoundSphereContext context) => _context = context;

        public IList<Album> FindAll() => _context.Albums.ToList();

        public Album FindById(Guid id) => _context.Albums.Find(id) ?? throw new Exception($"Album with id {id} not found!");

        public Album Save(Album album)
        {
            _context.Albums.Add(album);
            _context.SaveChanges();
            return album;
        }

        public Album UpdateById(Album album, Guid id)
        {
            Album albumToUpdate = FindById(id);
            _context.Entry(albumToUpdate).CurrentValues.SetValues(album);
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
    }
}