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

        public Album Save(Album album) => _albumRepository.Save(album);

        public Album UpdateById(Album album, Guid id) => _albumRepository.UpdateById(album, id);

        public Album DisableById(Guid id) => _albumRepository.DisableById(id);
    }
}