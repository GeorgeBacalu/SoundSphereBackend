using AutoMapper;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;

namespace SoundSphere.Core.Mappings
{
    public static class ArtistMappingExtensions
    {
        public static IList<ArtistDto> ToDtos(this IList<Artist> artists, IMapper mapper) => artists.Select(artist => artist.ToDto(mapper)).ToList();

        public static IList<Artist> ToEntities(this IList<ArtistDto> artistDtos, IMapper mapper) => artistDtos.Select(artistDto => artistDto.ToEntity(mapper)).ToList();

        public static ArtistDto ToDto(this Artist artist, IMapper mapper)
        {
            ArtistDto artistDto = mapper.Map<ArtistDto>(artist);
            artistDto.SimilarArtistsIds = artist.SimilarArtists.Select(artistLink => artistLink.SimilarArtistId).ToList();
            return artistDto;
        }

        public static Artist ToEntity(this ArtistDto artistDto, IMapper mapper)
        {
            Artist artist = mapper.Map<Artist>(artistDto);
            artist.SimilarArtists = artistDto.SimilarArtistsIds.Select(id => new ArtistLink { ArtistId = artistDto.Id, SimilarArtistId = id }).ToList();
            return artist;
        }
    }
}