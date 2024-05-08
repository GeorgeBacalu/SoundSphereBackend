using AutoMapper;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;

namespace SoundSphere.Core.Mappings
{
    public static class AlbumMappingExtensions
    {
        public static IList<AlbumDto> ToDtos(this IList<Album> albums, IMapper mapper) => albums.Select(album => album.ToDto(mapper)).ToList();

        public static IList<Album> ToEntities(this IList<AlbumDto> albumDtos, IMapper mapper) => albumDtos.Select(album => album.ToEntity(mapper)).ToList();

        public static AlbumDto ToDto(this Album album, IMapper mapper)
        {
            AlbumDto albumDto = mapper.Map<AlbumDto>(album);
            albumDto.SimilarAlbumsIds = album.SimilarAlbums.Select(albumLink => albumLink.SimilarAlbumId).ToList();
            return albumDto;
        }

        public static Album ToEntity(this AlbumDto albumDto, IMapper mapper)
        {
            Album album = mapper.Map<Album>(albumDto);
            album.SimilarAlbums = albumDto.SimilarAlbumsIds.Select(id => new AlbumLink { AlbumId = albumDto.Id, SimilarAlbumId = id }).ToList();
            return album;
        }
    }
}