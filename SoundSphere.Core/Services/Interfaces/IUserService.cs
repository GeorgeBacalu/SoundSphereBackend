using SoundSphere.Database.Dtos;
using SoundSphere.Database.Entities;

namespace SoundSphere.Core.Services.Interfaces
{
    public interface IUserService
    {
        IList<UserDto> FindAll();

        UserDto FindById(Guid id);

        UserDto Save(UserDto userDto);

        UserDto UpdateById(UserDto userDto, Guid id);

        UserDto DisableById(Guid id);

        IList<UserDto> ConvertToDtos(IList<User> users);

        IList<User> ConvertToEntities(IList<UserDto> userDtos);

        UserDto ConvertToDto(User user);

        User ConvertToEntity(UserDto userDto);
    }
}