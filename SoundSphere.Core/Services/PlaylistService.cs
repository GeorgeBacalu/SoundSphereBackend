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

        public Playlist Save(Playlist playlist) => _playlistRepository.Save(playlist);

        public Playlist UpdateById(Playlist playlist, Guid id) => _playlistRepository.UpdateById(playlist, id);

        public Playlist DisableById(Guid id) => _playlistRepository.DisableById(id);
    }
}