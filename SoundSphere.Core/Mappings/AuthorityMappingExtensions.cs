using AutoMapper;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;

namespace SoundSphere.Core.Mappings
{
    public static class AuthorityMappingExtensions
    {
        public static IList<AuthorityDto> ToDtos(this IList<Authority> authorities, IMapper mapper) => authorities.Select(authority => authority.ToDto(mapper)).ToList();

        public static IList<Authority> ToEntities(this IList<AuthorityDto> authorityDtos, IMapper mapper) => authorityDtos.Select(authorityDto => authorityDto.ToEntity(mapper)).ToList();

        public static AuthorityDto ToDto(this Authority authority, IMapper mapper) => mapper.Map<AuthorityDto>(authority);
        
        public static Authority ToEntity(this AuthorityDto authorityDto, IMapper mapper) => mapper.Map<Authority>(authorityDto);
    }
}