using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Database.Repositories
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IPlaylistRepository _playlistRepository;

        public PlaylistService(IPlaylistRepository playlistRepository) => _playlistRepository = playlistRepository;

        public IList<Playlist> FindAll() => _playlistRepository.FindAll();

        public Playlist FindById(Guid id) => _playlistRepository.FindById(id);

        public Playlist Save(Playlist playlist)
        {
            if (playlist == null) throw new Exception("Can't persist null playlist to DB!");
            if (playlist.Id == Guid.Empty) playlist.Id = Guid.NewGuid();
            playlist.IsActive = true;
            playlist.CreatedAt = DateTime.Now;
            return _playlistRepository.Save(playlist);
        }

        public Playlist UpdateById(Playlist playlist, Guid id)
        {
            if (playlist == null) throw new Exception("Can't persist null playlist to DB!");
            return _playlistRepository.UpdateById(playlist, id);
        }

        public Playlist DisableById(Guid id) => _playlistRepository.DisableById(id);
    }
}