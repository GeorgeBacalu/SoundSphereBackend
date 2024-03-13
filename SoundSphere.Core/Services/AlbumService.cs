using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Database.Repositories
{
    public class AlbumService : IAlbumService
    {
        private readonly IAlbumRepository _albumRepository;

        public AlbumService(IAlbumRepository albumRepository) => _albumRepository = albumRepository;

        public IList<Album> FindAll() => _albumRepository.FindAll();

        public Album FindById(Guid id) => _albumRepository.FindById(id);

        public Album Save(Album album)
        {
            if (album == null) throw new Exception("Can't persist null album to DB!");
            if (album.Id == Guid.Empty) album.Id = Guid.NewGuid();
            album.IsActive = true;
            return _albumRepository.Save(album);
        }

        public Album UpdateById(Album album, Guid id)
        {
            if (album == null) throw new Exception("Can't persist null album to DB!");
            return _albumRepository.UpdateById(album, id);
        }

        public Album DisableById(Guid id) => _albumRepository.DisableById(id);
    }
}