using AutoMapper;
using SoundSphere.Core.Services.Interfaces;
using SoundSphere.Database.Dtos;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Database.Repositories
{
    public class AlbumService : IAlbumService
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly IMapper _mapper;

        public AlbumService(IAlbumRepository albumRepository, IMapper mapper)
        {
            _albumRepository = albumRepository;
            _mapper = mapper;
        }

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

        public AlbumDto ConvertToDto(Album album)
        {
            AlbumDto albumDto = _mapper.Map<AlbumDto>(album);
            albumDto.SimilarAlbumsIds = album.SimilarAlbums
                .Select(albumLink => albumLink.SimilarAlbumId)
                .ToList();
            return albumDto;
        }

        public Album ConvertToEntity(AlbumDto albumDto)
        {
            Album album = _mapper.Map<Album>(albumDto);
            album.SimilarAlbums = albumDto.SimilarAlbumsIds
                .Select(id => new AlbumLink
                {
                    AlbumId = albumDto.Id,
                    SimilarAlbumId = id
                })
                .ToList();
            return album;
        }
    }
}