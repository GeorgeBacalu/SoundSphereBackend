using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Database.Repositories
{
    public class SongService : ISongService
    {
        private readonly ISongRepository _songRepository;
        private readonly IAlbumRepository _albumRepository;
        private readonly IArtistRepository _artistRepository;

        public SongService(ISongRepository songRepository, IAlbumRepository albumRepository, IArtistRepository artistRepository)
        {
            _songRepository = songRepository;
            _albumRepository = albumRepository;
            _artistRepository = artistRepository;
        }

        public IList<SongDto> FindAll() => ConvertToDtos(_songRepository.FindAll());

        public SongDto FindById(Guid id) => ConvertToDto(_songRepository.FindById(id));

        public SongDto Save(SongDto songDto)
        {
            Song song = ConvertToEntity(songDto);
            if (song.Id == Guid.Empty) song.Id = Guid.NewGuid();
            song.IsActive = true;
            _songRepository.LinkSongToAlbum(song);
            _songRepository.LinkSongToArtists(song);
            _songRepository.AddSongLink(song);
            _songRepository.AddUserSong(song);
            return ConvertToDto(_songRepository.Save(song));
        }

        public SongDto UpdateById(SongDto songDto, Guid id) => ConvertToDto(_songRepository.UpdateById(ConvertToEntity(songDto), id));

        public SongDto DisableById(Guid id) => ConvertToDto(_songRepository.DisableById(id));

        public IList<SongDto> ConvertToDtos(IList<Song> songs) => songs.Select(ConvertToDto).ToList();

        public IList<Song> ConvertToEntities(IList<SongDto> songDtos) => songDtos.Select(ConvertToEntity).ToList();

        public SongDto ConvertToDto(Song song) => new SongDto
        {
            Id = song.Id,
            Title = song.Title,
            ImageUrl = song.ImageUrl,
            Genre = song.Genre,
            ReleaseDate = song.ReleaseDate,
            DurationSeconds = song.DurationSeconds,
            AlbumId = song.Album.Id,
            ArtistsIds = song.Artists
                .Select(artist => artist.Id)
                .ToList(),
            SimilarSongsIds = song.SimilarSongs
                .Select(song => song.SimilarSongId)
                .ToList(),
            IsActive = song.IsActive
        };

        public Song ConvertToEntity(SongDto songDto) => new Song
        {
            Id = songDto.Id,
            Title = songDto.Title,
            ImageUrl = songDto.ImageUrl,
            Genre = songDto.Genre,
            ReleaseDate = songDto.ReleaseDate,
            DurationSeconds = songDto.DurationSeconds,
            Album = _albumRepository.FindById(songDto.AlbumId),
            Artists = songDto.ArtistsIds
                .Select(_artistRepository.FindById)
                .ToList(),
            SimilarSongs = songDto.SimilarSongsIds
                .Select(id => new SongLink
                {
                    SongId = songDto.Id,
                    SimilarSongId = id
                })
                .ToList(),
            IsActive = songDto.IsActive
        };
    }
}