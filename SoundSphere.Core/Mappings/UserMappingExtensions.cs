using AutoMapper;
using SoundSphere.Database.Dtos.Common;
using SoundSphere.Database.Entities;
using SoundSphere.Database.Repositories.Interfaces;

namespace SoundSphere.Core.Mappings
{
    public static class UserMappingExtensions
    {
        public static IList<UserDto> ToDtos(this IList<User> users, IMapper mapper) => users.Select(user => user.ToDto(mapper)).ToList();

        public static IList<User> ToEntities(this IList<UserDto> userDtos, IRoleRepository roleRepository, IAuthorityRepository authorityRepository, IMapper mapper) =>
            userDtos.Select(userDto => userDto.ToEntity(roleRepository, authorityRepository, mapper)).ToList();

        public static UserDto ToDto(this User user, IMapper mapper)
        {
            UserDto userDto = mapper.Map<UserDto>(user);
            userDto.AuthoritiesIds = user.Authorities.Select(authority => authority.Id).ToList();
            return userDto;
        }

        public static User ToEntity(this UserDto userDto, IRoleRepository roleRepository, IAuthorityRepository authorityRepository, IMapper mapper)
        {
            User user = mapper.Map<User>(userDto);
            user.Role = roleRepository.GetById(userDto.RoleId);
            user.Authorities = userDto.AuthoritiesIds.Select(authorityRepository.GetById).ToList();
            return user;
        }
    }
}