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

        public Song Save(Song song)
        {
            if (song == null) throw new Exception("Can't persist null song to DB!");
            if (song.Id == Guid.Empty) song.Id = Guid.NewGuid();
            song.IsActive = true;
            return _songRepository.Save(song);
        }

        public Song UpdateById(Song song, Guid id)
        {
            if (song == null) throw new Exception("Can't persist null song to DB!");
            return _songRepository.UpdateById(song, id);
        }

        public Song DisableById(Guid id) => _songRepository.DisableById(id);
    }
}