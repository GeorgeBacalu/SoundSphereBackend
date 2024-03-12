using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Database.Repositories
{
    public class SongService : ISongService
    {
        private readonly ISongRepository _songRepository; 
        
        public SongService(ISongRepository songRepository) => _songRepository = songRepository;

        public IList<Song> FindAll() => _songRepository.FindAll();

        public Song FindById(Guid id) => _songRepository.FindById(id);

        public Song Save(Song song) => _songRepository.Save(song);

        public Song UpdateById(Song song, Guid id) => _songRepository.UpdateById(song, id);

        public Song DisableById(Guid id) => _songRepository.DisableById(id);
    }
}