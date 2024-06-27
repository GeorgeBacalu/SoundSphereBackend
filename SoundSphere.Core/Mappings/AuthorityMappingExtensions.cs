using AutoMapper;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;

namespace SoundSphere.Core.Mappings
{
    public static class AuthorityMappingExtensions
    {
        public static IList<AuthorityDto> ToDtos(this IList<Authority> authorities, IMapper mapper)
        {
            IList<AuthorityDto> authorityDtos = authorities.Select(authority => authority.ToDto(mapper)).ToList();
            return authorityDtos;
        }

        public static IList<Authority> ToEntities(this IList<AuthorityDto> authorityDtos, IMapper mapper)
        {
            IList<Authority> authorities = authorityDtos.Select(authorityDto => authorityDto.ToEntity(mapper)).ToList();
            return authorities;
        }

        public static AuthorityDto ToDto(this Authority authority, IMapper mapper)
        {
            AuthorityDto authorityDto = mapper.Map<AuthorityDto>(authority);
            return authorityDto;
        }
        
        public static Authority ToEntity(this AuthorityDto authorityDto, IMapper mapper)
        {
            Authority authority = mapper.Map<Authority>(authorityDto);
            return authority;
        }
    }
}