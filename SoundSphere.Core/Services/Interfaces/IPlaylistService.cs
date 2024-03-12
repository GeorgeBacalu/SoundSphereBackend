using SoundSphere.Database.Entities;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IPlaylistService
    {
        IList<Playlist> FindAll();

        Playlist FindById(Guid id);

        Playlist Save(Playlist playlist);

        Playlist UpdateById(Playlist playlist, Guid id);

        Playlist DisableById(Guid id);
    }
}