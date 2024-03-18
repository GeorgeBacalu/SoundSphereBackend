using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Database.Repositories
{
    public class AlbumService : IAlbumService
    {
        private readonly IAlbumRepository _albumRepository;

        public AlbumService(IAlbumRepository albumRepository) => _albumRepository = albumRepository;

        public IList<AlbumDto> FindAll() => ConvertToDtos(_albumRepository.FindAll());

        public AlbumDto FindById(Guid id) => ConvertToDto(_albumRepository.FindById(id));

        public AlbumDto Save(AlbumDto albumDto)
        {
            Album album = ConvertToEntity(albumDto);
            if (album.Id == Guid.Empty) album.Id = Guid.NewGuid();
            album.IsActive = true;
            _albumRepository.AddAlbumLink(album);
            return ConvertToDto(_albumRepository.Save(album));
        }

        public AlbumDto UpdateById(AlbumDto albumDto, Guid id) => ConvertToDto(_albumRepository.UpdateById(ConvertToEntity(albumDto), id));

        public AlbumDto DisableById(Guid id) => ConvertToDto(_albumRepository.DisableById(id));

        public IList<AlbumDto> ConvertToDtos(IList<Album> albums) => albums.Select(ConvertToDto).ToList();

        public IList<Album> ConvertToEntities(IList<AlbumDto> albumDtos) => albumDtos.Select(ConvertToEntity).ToList();

        public AlbumDto ConvertToDto(Album album) => new AlbumDto
        {
            Id = album.Id,
            Title = album.Title,
            ImageUrl = album.ImageUrl,
            ReleaseDate = album.ReleaseDate,
            SimilarAlbumsIds = album.SimilarAlbums
                .Select(albumLink => albumLink.SimilarAlbumId)
                .ToList(),
            IsActive = album.IsActive
        };

        public Album ConvertToEntity(AlbumDto albumDto) => new Album
        {
            Id = albumDto.Id,
            Title = albumDto.Title,
            ImageUrl = albumDto.ImageUrl,
            ReleaseDate = albumDto.ReleaseDate,
            SimilarAlbums = albumDto.SimilarAlbumsIds
                .Select(id => new AlbumLink
                {
                    AlbumId = albumDto.Id,
                    SimilarAlbumId = id
                })
                .ToList(),
            IsActive = albumDto.IsActive
        };
    }
}